using logic.core.throughEvent;

namespace Assets.game.models.resource.limited
{
    public class LimitedResourceCategories
    {
        public readonly EventCategory changed = new EventCategory();
        public readonly EventCategory additionalMaximumChanged = new EventCategory();
    }
}
