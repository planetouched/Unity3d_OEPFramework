namespace Assets.logic.essential.reward.result
{
    public class WrappedRewardResult : RewardResult
    {
        public IRewardResult rewardResult { get; private set; }

        public WrappedRewardResult(string type, IRewardResult rewardResult)
            : base(type)
        {
            this.rewardResult = rewardResult;
        }
    }
}
