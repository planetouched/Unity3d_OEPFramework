using Assets.logic.core.throughEvent;

namespace Assets.logic.essential.price
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
