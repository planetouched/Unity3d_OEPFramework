using Basement.BLFramework.Core.ThroughEvent;

namespace Game.Models.Triggers
{
    public class TriggerCategories
    {
        public EventCategory completed = new EventCategory();
        public EventCategory activated = new EventCategory();
        public EventCategory update = new EventCategory();
    }
}