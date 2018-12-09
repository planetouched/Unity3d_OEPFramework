using Assets.common;
using Assets.logic.core.context;
using Assets.logic.essential.path;
using Assets.logic.essential.random;
using Assets.logic.essential.reward.result;

namespace Assets.logic.essential.reward
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
                random = PathUtil.ModelsPath(GetContext(), rawNode.GetString("random"), null).GetSelf<Random>();
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
