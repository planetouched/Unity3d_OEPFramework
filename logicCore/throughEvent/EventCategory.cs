namespace Assets.logicCore.throughEvent
{
    public class EventCategory : IEventCategory
    {
        private static int id;
        private readonly int hashCode;
        public string description { get; }

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
