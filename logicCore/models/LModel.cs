using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Assets.common;
using Assets.logicCore.contexts;
using Assets.logicCore.events;

namespace Assets.logicCore.models
{
    public abstract class LModel : IModel
    {
        private Event modelEvent;
        public WeakRef<IModel> weakParent { get; private set; }
        private readonly WeakRef<IContext> weakContext;

        protected IEventCategory eventCategory;
        protected OrderedDictionary children;

        protected LModel()
        {
        }

        protected LModel(IContext context)
        {
            weakContext = new WeakRef<IContext>(context);
        }

        protected IContext GetContext()
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

        private OrderedDictionary GetChildren()
        {
            return children ?? (children = new OrderedDictionary());
        }

        public IModel GetChild(string key)
        {
            return (IModel)children[key];
        }

        public T GetChild<T>(string key) where T : IModel
        {
            return (T)children[key];
        }

        public IModel AddChild(string key, IModel model, bool traverseEvents = true)
        {
            if (traverseEvents)
                model.SetParent(this);

            GetChildren().Add(key, model);
            return model;
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

        public void SetParent(IModel parent)
        {
            if (parent != null && weakParent != null && weakParent.obj != null)
                throw new Exception("объект может быть членом только одной иерархии");

            weakParent = new WeakRef<IModel>(parent);
        }

        public List<IModel> GetModelPath()
        {
            var models = new List<IModel>();
            IModel current = this;
            while (current != null)
            {
                models.Add(current);
                current = current.weakParent == null ? null : current.weakParent.obj;
            }
            return models;
        }

        public void Call(EventCategory category, IHandlerArgs args, IContext context)
        {
            var models = GetModelPath();
            Event.Call(category, models, args, context);
        }

        public IEnumerable<KeyValuePair<string, IModel>> GetCollection()
        {
            if (children != null)
            {
                foreach (DictionaryEntry pair in children)
                    yield return new KeyValuePair<string, IModel>((string)pair.Key, (IModel)pair.Value);
            }
        }

        public object Serialize()
        {
            var result = new Dictionary<string, object>();
            foreach (var pair in GetCollection())
            {
                object serialized = pair.Value.Serialize();
                if (serialized != null)
                    result.Add(pair.Key, serialized);
            }
            return result;
        }

        public virtual void Destroy()
        {
            if (modelEvent != null)
                modelEvent.Clear();

            SetParent(null);
            if (children == null) return;
            foreach (var pair in GetCollection())
            {
                pair.Value.Destroy();
            }

            children.Clear();
        }
    }
}
