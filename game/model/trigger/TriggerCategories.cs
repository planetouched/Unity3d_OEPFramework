using Assets.logic.core.throughEvent;

namespace Assets.game.model.trigger
{
    public class TriggerCategories
    {
        public EventCategory completed = new EventCategory();
        public EventCategory activated = new EventCategory();
        public EventCategory update = new EventCategory();
    }
}