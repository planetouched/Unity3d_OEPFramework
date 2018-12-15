using logic.core.throughEvent;

namespace logic.essential.price
{
    public interface IPrice
    {
        ModelsPath GetPath();
        string type { get; }
        int amount { get; }
        bool Check();
        void Pay();
    }
}
