namespace Assets.logicCore.throughEvent
{
    public interface IEventSource
    {
        Event GetEvent();
        void Attach(IEventCategory category, Event.EventHandler func);
        void Detach(IEventCategory category, Event.EventHandler func);
        void Call(IEventCategory category, IHandlerArgs args);
    }
}
