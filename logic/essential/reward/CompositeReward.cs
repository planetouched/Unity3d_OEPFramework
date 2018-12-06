using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.collection;
using Assets.logic.essential.reward.result;

namespace Assets.logic.essential.reward
{
    public class CompositeReward : Reward
    {
        public LazyArray<Reward> rewards { get; private set; }

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
