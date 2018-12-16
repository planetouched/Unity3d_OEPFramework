using common;
using logic.core.context;
using logic.core.reference.collection;
using logic.essential.reward.result;

namespace logic.essential.reward
{
    public class CompositeReward : Reward
    {
        public LazyArray<Reward> rewards { get; }

        public CompositeReward(RawNode rawNode, IContext context)
            : base(rawNode, context)
        {
            rewards = new LazyArray<Reward>(rawNode.GetNode("rewards"), context);
        }

        public  override IRewardResult Calculate()
        {
            var rewardResults = new IRewardResult[rewards.Count()];

            for (int i = 0; i < rewards.Count(); i++)
            {
                rewardResults[i] = rewards[i].Calculate();
            }

            return new CompositeRewardResult(type, rewardResults);
        }

        public override IRewardResult Award(IRewardResult rewardResult)
        {
            var results = ((CompositeRewardResult)rewardResult).results;

            for (int i = 0; i < results.Length; i++)
            {
                rewards[i].Award(results[i]);
            }

            return rewardResult;
        }
    }
}
