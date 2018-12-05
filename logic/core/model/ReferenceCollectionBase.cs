using System.Collections.Generic;
using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.dataSource;
using Assets.logic.core.reference.description;

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

        public override IModel GetChild(string key)
        {
            return this[key];
        }

        protected abstract TModel Factory(RawNode initNode, TDescription description, IContext context);

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
    }
}
