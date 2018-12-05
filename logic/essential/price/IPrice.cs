using Assets.logic.essential.path;

namespace Assets.logic.essential.price
{
    public interface IPrice
    {
        Path GetPath();
        string type { get; }
        int amount { get; }
        bool Check();
        void Pay();
    }
}
