using Basement.BLFramework.Core.ThroughEvent;

namespace Game.Models.Resources.Limited
{
    public class LimitedResourceCategories
    {
        public readonly EventCategory changed = new EventCategory();
        public readonly EventCategory additionalMaximumChanged = new EventCategory();
    }
}
