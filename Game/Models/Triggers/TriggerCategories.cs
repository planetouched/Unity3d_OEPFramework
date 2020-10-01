using Basement.BLFramework.Core.ThroughEvent;
using Game.Models.Triggers.Associations;

namespace Game.Models.Triggers
{
    public class TriggerCategories
    {
        public readonly EventCategory enabled = new EventCategory();
        public readonly EventCategory disabled = new EventCategory();
        public readonly EventCategory completed = new EventCategory();
        public readonly EventCategory activated = new EventCategory();
        public readonly EventCategory deactivated = new EventCategory();
        public readonly EventCategory accepted = new EventCategory();
        
        public readonly AssociationCategories association = new AssociationCategories();
    }
}