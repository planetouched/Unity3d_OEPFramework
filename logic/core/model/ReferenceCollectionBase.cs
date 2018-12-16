using System.Collections.Generic;
using common;
using logic.core.context;
using logic.core.reference.description;
using logic.core.util;

namespace logic.core.model
{
    public abstract class ReferenceCollectionBase<TModel, TCategories, TDescription> : CollectionBase<TModel>, IReferenceModel, IEnumerable<KeyValuePair<string, TModel>>, IReferenceCollection
        where TDescription : IDescription
        where TCategories : class
        where TModel : IReferenceModel
    {
        public TCategories categories { get; }
        public IDescription dataSource { get; }
        private readonly RawNode initNode;
        public bool selectable { get; }

        protected ReferenceCollectionBase(RawNode initNode, TCategories categories, IContext context, IDescription dataSource) : base(context, null)
        {
            key = dataSource.key;
            this.initNode = initNode;
            this.dataSource = dataSource;
            this.categories = categories;
            selectable = true;
        }

        public override TModel this[string collectionKey]
        {
            get
            {
                TModel model = base[collectionKey];

                if (model != null)
                {
                    return model;
                }

                var description = (TDescription)dataSource.GetChild(collectionKey);
                model = Factory(initNode.GetNode(collectionKey), description);
                AddChild(collectionKey, model);
                description.Initialization();
                model.Initialization();

                return model;
            }
        }

        public new IEnumerator<KeyValuePair<string, TModel>> GetEnumerator()
        {
            foreach (var sortedKey in dataSource.GetNode().GetSortedKeys())
            {
                yield return new KeyValuePair<string, TModel>(sortedKey, this[sortedKey]);
            }
        }

        public override object Serialize()
        {
            var dict = SerializeUtil.Dict();

            foreach (var unsortedKey in dataSource.GetNode().GetUnsortedKeys())
            {
                if (Exist(unsortedKey))
                {
                    var serialized = this[unsortedKey].Serialize();

                    if (serialized != null)
                    {
                        dict.Add(unsortedKey, serialized);
                    }
                }
                else
                {
                    dict.Add(unsortedKey, initNode.GetNode(unsortedKey));
                }
            }

            return dict;
        }

        protected abstract TModel Factory(RawNode initNode, TDescription description);
    }
}
