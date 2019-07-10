using Basement.BLFramework.Essential.Reward.Result;

namespace Basement.BLFramework.Essential.Reward
{
    public interface IReward
    {
        string type { get; }
        IRewardResult Award();
        IRewardResult Award(IRewardResult rewardResult);
        IRewardResult Calculate();
    }
}
