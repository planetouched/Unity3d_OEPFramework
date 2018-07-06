using Assets.logicCore.contexts;

namespace Assets.logicCore.events
{
    public struct CoreParams
    {
        public IContext context;
        public EventCallStack stack;
        public EventCategory category;
    }
}
