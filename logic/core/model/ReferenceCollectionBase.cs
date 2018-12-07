using System.Collections.Generic;
using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.dataSource;
using Assets.logic.core.reference.description;
using Assets.logic.core.util;

namespace Assets.logic.core.model
{
    public abstract class ReferenceCollectionBase<TModel, TDescription> : CollectionBase<TModel>, IReferenceCollection
        where TDescription : ISelectableDescription
        where TModel : IModel
    {

        private readonly IDataSource<string, TDescription> dataSource;

        private readonly RawNode initNode;

        protected ReferenceCollectionBase(RawNode initNode, IContext context, IDataSource<string, TDescription> dataSource) : base(context, null)
        {
            key = dataSource.key;
            this.initNode = initNode;
            this.dataSource = dataSource;
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

                var description = dataSource.GetDescription(collectionKey);
                model = Factory(initNode.GetNode(collectionKey), description, GetContext());
                AddChild(collectionKey, model);
                model.Initialization();

                return model;
            }
        }

        public override IModel GetChild(string collectionKey)
        {
            return this[collectionKey];
        }

        public IEnumerable<string> GetUnsortedKeys()
        {
            foreach (var pair in dataSource.GetNode().GetUnsortedCollection())
                yield return pair.Key;
        }

        public IEnumerable<string> GetSortedKeys()
        {
            foreach (var pair in dataSource.GetNode().GetSortedCollection())
                yield return pair.Key;
        }

        public override IEnumerator<KeyValuePair<string, TModel>> GetEnumerator()
        {
            foreach (var sortedKey in GetSortedKeys())
            {
                yield return new KeyValuePair<string, TModel>(sortedKey, this[sortedKey]);
            }
        }

        public override object Serialize()
        {
            var dict = SerializeUtil.Dict();

            foreach (var unsortedKey in GetUnsortedKeys())
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

        protected abstract TModel Factory(RawNode initNode, TDescription description, IContext context);
    }
}
