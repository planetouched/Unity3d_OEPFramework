using logic.core.throughEvent;

namespace game.model.resource.limited
{
    public class LimitedResourceCategories
    {
        public readonly EventCategory changed = new EventCategory();
        public readonly EventCategory additionalMaximumChanged = new EventCategory();
    }
}
