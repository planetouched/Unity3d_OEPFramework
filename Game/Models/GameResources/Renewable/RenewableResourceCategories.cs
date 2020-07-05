using Basement.BLFramework.Core.ThroughEvent;

namespace Game.Models.GameResources.Renewable
{
    public class RenewableResourceCategories
    {
        public EventCategory Changed { get; } = new EventCategory();
    }
}
