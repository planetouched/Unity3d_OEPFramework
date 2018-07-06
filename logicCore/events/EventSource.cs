using Assets.logicCore.contexts;

namespace Assets.logicCore.events
{
    public interface IEventSource
    {
        Event GetEvent();
        void Attach(EventCategory category, Event.EventHandler func);
        void Detach(EventCategory category, Event.EventHandler func);
        void Call(EventCategory category, IHandlerArgs args, IContext context);
    }
}
