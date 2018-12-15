using System;
using System.Collections.Generic;
using common;
using logic.core.context;
using logic.core.throughEvent;

namespace logic.core.model
{
    public abstract class ModelBase : IModel
    {
        public string key { get; set; }
        protected object eventCategory;
        private readonly WeakRef<IContext> weakContext;
        private WeakRef<IModel> weakParent;
        private Event modelEvent;

        protected ModelBase(IContext context, IModel parent = null)
        {
            weakContext = new WeakRef<IContext>(context);

            if (parent != null)
            {
                SetParent(parent);
            }
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

        public void SetParent(IModel newParent)
        {
            var parent = GetParent();
            if (newParent == parent) return;

            if (newParent != null && parent != null && newParent != parent)
                throw new Exception("объект может быть членом только одной иерархии");

            weakParent = new WeakRef<IModel>(newParent);
        }

        public IModel GetParent()
        {
            if (weakParent != null)
            {
                return weakParent.obj;
            }

            return null;
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
