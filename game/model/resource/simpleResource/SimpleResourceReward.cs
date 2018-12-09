using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.factories;
using Assets.logic.essential.amount;
using Assets.logic.essential.reward;
using Assets.logic.essential.reward.result;

namespace Assets.game.model.resource.simpleResource
{
    public class SimpleResourceReward : Reward
    {
        private readonly IAmount amount;

        public SimpleResourceReward(RawNode node, IContext context = null) : base(node, context)
        {
            amount = FactoryManager.Build<Amount>(node.GetNode("amount"), context);
        }

        public override IRewardResult Calculate()
        {
            var path = RewardPath();

            if (path == null)
                return new RewardResult();

            var simpleResource = path.GetSelf<SimpleResource>();
            int value = amount.Number();
            return new SimpleResourceRewardResult(type, simpleResource, value, GetContext());
        }

        public override IRewardResult Award(IRewardResult rewardResult)
        {
            var simpleResourceRewardResult = (SimpleResourceRewardResult) rewardResult;
            simpleResourceRewardResult.resource.Increment(simpleResourceRewardResult.amount);
            return rewardResult;
        }
    }
}
