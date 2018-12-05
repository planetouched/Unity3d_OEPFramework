using Assets.logic.essential.path;

namespace Assets.logic.essential.requirement
{
    public interface IRequirement
    {
        string type { get; }
        Path GetPath();
        bool Check();
    }
}