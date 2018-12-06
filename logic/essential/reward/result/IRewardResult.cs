using Assets.logic.core.context;
using Assets.logic.core.model;

namespace Assets.logic.essential.reward.result
{
    public interface IRewardResult : ISerialize, IHasContext
    {
        string type { get; }
    }
}