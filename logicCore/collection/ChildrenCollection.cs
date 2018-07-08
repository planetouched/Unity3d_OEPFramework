using System;
using System.Collections.Specialized;
using Assets.common;

namespace Assets.logicCore.collection
{
    public abstract class ChildrenCollection : IChild
    {
        protected OrderedDictionary children;
        public WeakRef<IChild> weakParent { get; private set; }
        public string id { get; }

        protected ChildrenCollection(string id)
        {
            this.id = id;
        }

        private OrderedDictionary GetChildren()
        {
            return children ?? (children = new OrderedDictionary());
        }

        public IChild GetChild(string key)
        {
            return (IChild)children[key];
        }

        public T GetChild<T>(string key) where T : IChild
        {
            return (T)children[key];
        }

        public IChild AddChild(string key, IChild obj, bool traverseEvents = true)
        {
            if (traverseEvents)
                obj.SetParent(this);

            GetChildren().Add(key, obj);
            return obj;
        }

        public void RemoveChild(string key)
        {
            if (children == null) return;
            if (children.Contains(key))
            {
                GetChild(key).SetParent(null);
                children.Remove(key);
            }
        }

        public void SetParent(IChild parent)
        {
            if (parent != null && weakParent != null && weakParent.obj != null)
                throw new Exception("объект может быть членом только одной иерархии");

            weakParent = new WeakRef<IChild>(parent);
        }
    }
}
