using Basement.BLFramework.Core.Reference.Description;
using Basement.BLFramework.Core.ThroughEvent;

namespace Basement.BLFramework.Essential.Choice
{
    public interface IPathChoice
    {
        ModelsPath GetModelPath();
        T GetDescription<T>() where T : class, IDescription;
    }
}
