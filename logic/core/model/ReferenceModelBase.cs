using System.Collections.Generic;
using common;
using logic.core.context;
using logic.core.reference.description;

namespace logic.core.model
{
    public abstract class ReferenceModelBase<TCategories, TDescription> : ModelBase, IReferenceModel
        where TDescription : ISelectableDescription
        where TCategories : class
    {
        public ISelectableDescription description { get; protected set; }
        public TCategories categories { get; protected set; }

        public bool selectable { get; private set; }
        protected RawNode initNode;
        private IDictionary<string, IModel> children;

        protected ReferenceModelBase(RawNode initNode, TCategories categories, TDescription description, IContext context) : base(context)
        {
            key = description.key;
            this.initNode = initNode;
            this.description = description;
            this.categories = categories;
        }

        public TDescription GetDescription()
        {
            return (TDescription) description;
        }

        private IDictionary<string, IModel> GetChildren()
        {
            return children ?? (children = new Dictionary<string, IModel>());
        }

        public void AddChild(string collectionKey, IModel model)
        {
            GetChildren().Add(collectionKey, model);
            model.SetParent(this);
        }

        public IModel GetChild(string collectionKey)
        {
            return GetChildren()[collectionKey];
        }

        public void RemoveChild(string collectionKey)
        {
            if (children == null) return;

            if (children.ContainsKey(collectionKey))
            {
                GetChild(collectionKey).SetParent(null);
                children.Remove(collectionKey);
            }
        }

        public bool Exist(string collectionKey)
        {
            return children.ContainsKey(collectionKey);
        }

        public int Count()
        {
            return children.Count;
        }
    }
}