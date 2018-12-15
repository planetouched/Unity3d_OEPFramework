using logic.core.throughEvent;

namespace logic.essential.requirement
{
    public interface IRequirement
    {
        string type { get; }
        ModelsPath GetPath();
        bool Check();
    }
}