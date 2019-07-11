using Basement.BLFramework.Core.ThroughEvent;

namespace Game.Models.GameResources.Limited
{
    public class LimitedResourceCategories
    {
        public readonly EventCategory changed = new EventCategory();
        public readonly EventCategory additionalMaximumChanged = new EventCategory();
    }
}
