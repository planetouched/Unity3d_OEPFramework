using Assets.logic.core.throughEvent;

namespace Assets.logic.essential.requirement
{
    public interface IRequirement
    {
        string type { get; }
        ModelsPath GetPath();
        bool Check();
    }
}