using System.Collections.Generic;
using common;
using logic.core.context;

namespace logic.core.reference.description
{
    public abstract class DescriptionBase : IDescription
    {
        protected readonly RawNode node;
        private readonly WeakRef<IContext> weakContext;
        private WeakRef<IDescription> weakParent;
        private IDictionary<string, IDescription> items;
        public bool selectable { get; }
        public string key { get; }

        protected DescriptionBase(RawNode node, IContext context = null)
        {
            this.node = node;
            key = node.nodeKey;
            selectable = node.GetBool("selectable", true);

            if (context != null)
            {
                weakContext = new WeakRef<IContext>(context);
            }
        }

        public string GetDescriptionPath()
        {
            var path = new List<string>();
            IDescription current = this;
            
            while (current != null)
            {
                path.Add(current.key);
                current = current.GetParent();
            }
            
            path.Reverse();
            return string.Join(".", path.ToArray());
        }

        public virtual void Initialization()
        {
        }

        protected IDictionary<string, IDescription> GetChildren()
        {
            return items ?? (items = new Dictionary<string, IDescription>());
        }
        
        public virtual IDescription GetChild(string collectionKey)
        {
            return GetChildren()[collectionKey];
        }

        public void AddChild(string collectionKey, IDescription description)
        {
            GetChildren().Add(collectionKey, description);
            description.SetParent(this);
        }

        public virtual void RemoveChild(string collectionKey, bool destroy)
        {
            var description = GetChildren()[collectionKey];
            description.SetParent(null);
            GetChildren().Remove(collectionKey);
        }

        public bool Exist(string collectionKey)
        {
            return GetChildren().ContainsKey(collectionKey);
        }

        public RawNode GetNode()
        {
            return node;
        }

        public IContext GetContext()
        {
            return weakContext.obj;
        }

        public void SetParent(IDescription parent)
        {
            if (parent == null)
            {
                weakParent = null;
                return;
            }
            
            weakParent = new WeakRef<IDescription>(parent);
        }

        public IDescription GetParent()
        {
            return weakParent == null ? null : weakParent.obj;
        }
    }
}
