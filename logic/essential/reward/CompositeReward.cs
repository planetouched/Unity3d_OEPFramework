using System.Collections.Generic;
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

        protected override IRewardResult OnAward()
        {
            var rewardResults = new List<IRewardResult>();

            foreach (var reward in rewards)
            {
                rewardResults.Add(reward.Value.Award());
            }

            return new CompositeRewardResult(type, rewardResults);
        }
    }
}
