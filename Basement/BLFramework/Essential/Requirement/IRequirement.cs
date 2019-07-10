using Basement.BLFramework.Core.ThroughEvent;

namespace Basement.BLFramework.Essential.Requirement
{
    public interface IRequirement
    {
        string type { get; }
        ModelsPath GetPath();
        bool Check();
    }
}