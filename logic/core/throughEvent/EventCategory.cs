namespace Assets.logic.core.throughEvent
{
    public class EventCategory
    {
        private static int id;
        private readonly int hashCode;
        public string description { get; private set; }

        public EventCategory(string description = null)
        {
            hashCode = id++;
            this.description = description;
        }

        public override int GetHashCode()
        {
            return hashCode;
        }
    }
}
