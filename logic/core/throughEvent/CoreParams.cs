using Assets.logic.core.context;

namespace Assets.logic.core.throughEvent
{
    public struct CoreParams
    {
        public IContext context;
        public EventCallStack stack;
        public EventCategory category;
    }
}
