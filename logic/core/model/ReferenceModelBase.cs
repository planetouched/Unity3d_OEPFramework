using System.Collections.Generic;
using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.description;

namespace Assets.logic.core.model
{
    public abstract class ReferenceModelBase<TDescription> : ModelBase, IReferenceModel where TDescription : ISelectableDescription
    {
        public ISelectableDescription description { get; private set; }
        public bool selectable { get; private set; }
        protected RawNode initNode;
        private IDictionary<string, IModel> children;

        protected ReferenceModelBase(RawNode initNode, TDescription description, IContext context) : base(context)
        {
            key = description.key;
            this.initNode = initNode;
            this.description = description;
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