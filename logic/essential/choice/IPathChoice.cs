using logic.core.reference.description;
using logic.core.throughEvent;

namespace logic.essential.choice
{
    public interface IPathChoice
    {
        ModelsPath GetModelPath();
        T GetDescription<T>() where T : class, IDescription;
    }
}
