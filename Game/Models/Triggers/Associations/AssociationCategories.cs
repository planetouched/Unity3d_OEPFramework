using Basement.BLFramework.Core.ThroughEvent;

namespace Game.Models.Triggers.Associations
{
    public class AssociationCategories
    {
        public readonly EventCategory changed = new EventCategory();
        public readonly EventCategory complete = new EventCategory();
    }
}