using common;
using logic.core.context;
using logic.essential.path;
using logic.essential.random;
using logic.essential.reward.result;

namespace logic.essential.reward
{
    public class RandomReward : WrappedReward
    {
        private readonly Random random;
        public double probability { get; private set; }

        public RandomReward(RawNode rawNode, IContext context)
            : base(rawNode, context)
        {
            if (rawNode.CheckKey("random"))
            {
                random = PathUtil.GetModelPath(GetContext(), rawNode.GetString("random"), null).GetSelf<Random>();
            }

            probability = rawNode.GetDouble("probability");
        }

        public override IRewardResult Calculate()
        {
            var check = random.NextDouble() <= probability;
            var result = check ? new WrappedRewardResult(type, innerReward.Calculate()) : new RewardResult();
            return result;
        }

        public override IRewardResult Award(IRewardResult rewardResult)
        {
            innerReward.Award(((WrappedRewardResult)rewardResult).rewardResult);
            return rewardResult;
        }
    }
}
