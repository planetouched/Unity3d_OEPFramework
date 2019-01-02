using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using common;
using logic.core.context;
using logic.core.throughEvent;

namespace logic.core.model
{
    public abstract class ModelBase : IModel, IEnumerable<KeyValuePair<string, IModel>>
    {
        public string key { get; set; }
        private readonly WeakRef<IContext> weakContext;
        private WeakRef<IModel> weakParent;
        private Event modelEvent;
        private IOrderedDictionary children;

        protected ModelBase(IContext context, IModel parent = null)
        {
            weakContext = new WeakRef<IContext>(context);

            if (parent != null)
            {
                SetParent(parent);
            }
        }

        protected IDictionary GetChildren()
        {
            return children ?? (children = new OrderedDictionary());
        }
        
        public IContext GetContext()
        {
            return weakContext.obj;
        }

        public Event GetEvent()
        {
            return modelEvent ?? (modelEvent = new Event());
        }

        public void Attach(EventCategory category, Event.EventHandler func)
        {
            GetEvent().Attach(category, func);
        }

        public void Detach(EventCategory category, Event.EventHandler func)
        {
            GetEvent().Detach(category, func);
        }

        public virtual void Initialization()
        {
        }

        public void SetParent(IModel parent)
        {
            if (parent == null)
            {
                weakParent = null;
                return;
            }
            
            weakParent = new WeakRef<IModel>(parent);
        }

        public IModel GetParent()
        {
            return weakParent != null ? weakParent.obj : null;
        }

        public virtual IModel GetChild(string collectionKey)
        {
            return (IModel)GetChildren()[collectionKey];
        }

        public virtual void AddChild(string collectionKey, IModel model)
        {
            GetChildren().Add(collectionKey, model);
            model.SetParent(this);
        }

        public void RemoveChild(string collectionKey, bool destroy)
        {
            if (children == null) return;

            var child = GetChild(collectionKey);
            child.SetParent(null);
            children.Remove(collectionKey);

            if (destroy)
            {
                child.Destroy();
            }
        }

        public bool Exist(string collectionKey)
        {
            return GetChildren().Contains(collectionKey);
        }
        
        public virtual bool CheckAvailable()
        {
            return true;
        }

        public IList<IModel> GetModelPath(bool check)
        {
            var models = new List<IModel>();
            IModel current = this;

            while (current != null)
            {
                if (check && !current.CheckAvailable())
                    return null;

                models.Add(current);
                current = current.GetParent();
            }

            return models;
        }

        public IEnumerator<KeyValuePair<string, IModel>> GetEnumerator()
        {
            foreach (DictionaryEntry pair in GetChildren())
            {
                yield return new KeyValuePair<string, IModel>((string)pair.Key, (IModel)pair.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public void Call(EventCategory category, object args)
        {
            var models = GetModelPath(true);
            Event.Call(category, models, args, GetContext());
        }

        public virtual object Serialize()
        {
            return null;
        }

        public virtual void Destroy()
        {
            if (modelEvent != null)
            {
                modelEvent.Clear();
            }

            SetParent(null);
        }
    }
}
