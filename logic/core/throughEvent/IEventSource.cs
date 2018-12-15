﻿namespace logic.core.throughEvent
{
    public interface IEventSource
    {
        Event GetEvent();
        void Attach(EventCategory category, Event.EventHandler func);
        void Detach(EventCategory category, Event.EventHandler func);
        void Call(EventCategory category, object args);
    }
}
