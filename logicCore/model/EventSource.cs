using System.Collections.Generic;
using Assets.logicCore.collection;
using Assets.logicCore.throughEvent;

namespace Assets.logicCore.model
{
    public abstract class EventSource : ChildrenCollection, IEventSource
    {
        protected Event throughEvent;

        protected EventSource(string id) : base(id)
        {
        }

        public Event GetEvent()
        {
            return throughEvent ?? (throughEvent = new Event());
        }

        public void Attach(IEventCategory category, Event.EventHandler func)
        {
            GetEvent().Attach(category, func);
        }

        public void Detach(IEventCategory category, Event.EventHandler func)
        {
            GetEvent().Detach(category, func);
        }

        public List<IEventSource> GetPath()
        {
            var children = new List<IEventSource>();
            EventSource current = this;
            while (current != null)
            {
                children.Add(current);
                current = current.weakParent == null ? null : (EventSource)current.weakParent.obj;
            }
            return children;
        }

        public void Call(IEventCategory category, IHandlerArgs args)
        {
            Event.Call(category, GetPath(), args);
        }
    }
}
