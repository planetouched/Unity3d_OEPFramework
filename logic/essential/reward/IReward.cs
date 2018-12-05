using Assets.logic.essential.reward.result;

namespace Assets.logic.essential.reward
{
    public interface IReward
    {
        string type { get; }
        IRewardResult Award();
    }
}
