using System;
using System.Collections.Generic;
using common;
using logic.core.context;
using logic.core.model;
using logic.core.throughEvent;
using Random = logic.essential.random.Random;

namespace logic.essential.path
{
    public static class PathUtil
    {
        public static string StringPath(IModel model)
        {
            return string.Join(".", GetSeparatedPath(model));
        }

        private static string[] GetSeparatedPath(IModel model)
        {
            var models = model.GetModelPath(false);
            var arr = new string[models.Count];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[arr.Length - i - 1] = models[i].key;
            }
            return arr;
        }

        public static ModelsPath ModelsPath(IContext context, RawNode node)
        {
            string path = node.CheckKey("path") ? node.GetString("path") : node.ToString();
            Random random = null;

            if (node.CheckKey("random"))
            {
                random = ModelsPath(context, node.GetString("random"), null).GetSelf<Random>();
            }

            return GetResult(context, GetParts(path), random);
        }

        public static ModelsPath ModelsPath(IContext context, string path, Random random)
        {
            return GetResult(context, GetParts(path), random);
        }

        private static ModelsPath GetResult(IContext context, IList<string[]> parts, Random random)
        {
            var models = new List<IModel>();
            var result = new ModelsPath();

            for (int i = 0; i < parts.Count; i++)
            {
                IReferenceCollection collection = null;
                IReferenceModel model;

                if (i == 0)
                {
                    collection = (IReferenceCollection)context.GetChild(parts[0][0]);
                }

                if (SelectPathUtil.IsSimple(parts[i][1]))
                {
                    model = (IReferenceModel)collection.GetChild(parts[i][1]);
                    models.Add(collection);
                    models.Add(model);
                }
                else
                {
                    var allKeys = new List<string>();
                    allKeys.AddRange(collection.GetSortedKeys());
                    var affectedKeys = SelectPathUtil.GetAffectedKeys(parts[i][1], allKeys);
                    var availableModels = new List<IReferenceModel>();

                    foreach (var key in affectedKeys)
                    {
                        var testModel = (IReferenceModel)collection.GetChild(key);
                        if (testModel.description.canSelect && testModel.CheckAvailable())
                            availableModels.Add(testModel);
                    }

                    if (availableModels.Count == 0)
                    {
                        return null;
                    }

                    model = availableModels[random.Range(0, availableModels.Count)];
                    models.Add(collection);
                    models.Add(model);
                }
            }

            result.Set(models, false);
            return result;
        }

        private static IList<string[]> GetParts(string path)
        {
            var selectors = new List<string[]>();
            string[] parts = path.Split('.');
            if (parts.Length % 2 == 1)
                throw new Exception("Путь содержит нечетное количество параметров: " + path);

            for (int i = 0; i < parts.Length; i += 2)
                selectors.Add(new[] { parts[i], parts[i + 1] });
            return selectors;
        }

        public static RawNode RawNodePath(IContext context, string path, Random random)
        {
            var parts = GetParts(path);

            RawNode current = context.repositoryNode;

            for (int i = 0; i < parts.Count; i++)
            {
                current = current.GetNode(parts[i][0]);

                if (SelectPathUtil.IsSimple(parts[i][1]))
                {
                    current = current.GetNode(parts[i][1]);
                }
                else
                {
                    var allKeys = new List<string>();
                    allKeys.AddRange(current.GetSortedKeys());
                    var affectedKeys = SelectPathUtil.GetAffectedKeys(parts[i][1], allKeys);
                    if (affectedKeys.Count == 0)
                    {
                        return null;
                    }

                    var selectedKey = affectedKeys[random.Range(0, affectedKeys.Count)];
                    current = current.GetNode(selectedKey);
                }
            }

            return current;
        }
    }
}
