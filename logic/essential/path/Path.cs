using System;
using System.Collections.Generic;
using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.model;
using Assets.logic.core.throughEvent;
using Random = Assets.logic.essential.random.Random;

namespace Assets.logic.essential.path
{
    public class Path
    {
        public EventCallStack result { get; private set; }

        private Path(EventCallStack result)
        {
            this.result = result;
        }

        public static string StringPath(IModel model)
        {
            return string.Join(".", GetSeparatedPath(model));
        }

        public static string[] GetSeparatedPath(IModel model)
        {
            var models = model.GetModelPath(false);
            var sPath = new List<string>();
            for (int i = models.Count - 1; i >= 0; i--)
            {
                var current = models[i];
                sPath.Add(current.key);
            }
            return sPath.ToArray();
        }

        public static Path Create(IContext context, RawNode node)
        {
            string path = node.CheckKey("selector") ? node.GetString("selector") : node.ToString();
            IList<string[]> parts = Init(path);

            if (node.CheckKey("random"))
            {
                Path randomPath = Create(context, node.GetString("random"), null);
                return GetRandomResult(context, parts, randomPath.result.GetSelf<Random>());
            }

            return GetResult(context, parts);
        }

        public static Path Create(IContext context, string path, Random random)
        {
            IList<string[]> parts = Init(path);

            if (random == null)
            {
                return GetResult(context, parts);
            }

            return GetRandomResult(context, parts, random);
        }

        private static Path GetResult(IContext context, IList<string[]> parts)
        {
            var models = new List<IModel>();
            var result = new EventCallStack();

            IReferenceModel model = null;

            for (int i = 0; i < parts.Count; i++)
            {
                IReferenceCollection collection;

                if (i == 0)
                {
                    collection = (IReferenceCollection)context.GetChild(parts[0][0]);
                }
                else
                {
                    collection = (IReferenceCollection)model.GetChild(parts[i][0]);
                }

                model = (IReferenceModel)collection.GetChild(parts[i][1]);
                models.Add(collection);
                models.Add(model);
            }

            result.Set(models, false);
            return new Path(result);
        }

        private static Path GetRandomResult(IContext context, IList<string[]> parts, Random random)
        {
            var models = new List<IModel>();
            var result = new EventCallStack();

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
            return new Path(result);
        }

        private static IList<string[]> Init(string path)
        {
            var selectors = new List<string[]>();
            string[] parts = path.Split('.');
            if (parts.Length % 2 == 1)
                throw new Exception("Путь содержит нечетное количество параметров: " + path);

            for (int i = 0; i < parts.Length; i += 2)
                selectors.Add(new[] { parts[i], parts[i + 1] });
            return selectors;
        }

        public override string ToString()
        {
            return StringPath(result.GetSelf());
        }
    }
}
