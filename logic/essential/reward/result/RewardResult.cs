namespace Assets.logic.essential.reward.result
{
    public class RewardResult : IRewardResult
    {
        public string type { get; private set; }

        public RewardResult(string type)
        {
            this.type = type;
        }

    }
}
