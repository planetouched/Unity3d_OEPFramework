using Basement.BLFramework.Core.ThroughEvent;

namespace Basement.BLFramework.Essential.Price
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
