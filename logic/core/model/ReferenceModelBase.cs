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

        public void AddChild(string key, IModel model)
        {
            GetChildren().Add(key, model);
            model.SetParent(this);
        }

        public IModel GetChild(string key)
        {
            return GetChildren()[key];
        }

        public void RemoveChild(string key)
        {
            if (children == null) return;

            if (children.ContainsKey(key))
            {
                GetChild(key).SetParent(null);
                children.Remove(key);
            }
        }

        public bool Exist(string key)
        {
            return children.ContainsKey(key);
        }

        public int Count()
        {
            return children.Count;
        }
    }
}