using logic.essential.reward.result;

namespace logic.essential.reward
{
    public interface IReward
    {
        string type { get; }
        IRewardResult Award();
        IRewardResult Award(IRewardResult rewardResult);
        IRewardResult Calculate();
    }
}
