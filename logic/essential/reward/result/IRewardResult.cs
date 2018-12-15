using logic.core.context;
using logic.core.model;

namespace logic.essential.reward.result
{
    public interface IRewardResult : ISerialize, IHasContext
    {
        string type { get; }
    }
}