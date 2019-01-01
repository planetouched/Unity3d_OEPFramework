using common;
using logic.core.context;
using logic.core.factories;
using logic.essential.amount;
using logic.essential.reward;
using logic.essential.reward.result;

namespace Assets.game.models.resources.simple
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

            var resource = path.GetSelf<SimpleResource>();
            int value = amount.Number();
            return new SimpleResourceRewardResult(type, resource, value, GetContext());
        }

        public override IRewardResult Award(IRewardResult rewardResult)
        {
            var simpleResourceRewardResult = (SimpleResourceRewardResult) rewardResult;
            simpleResourceRewardResult.resource.Change(simpleResourceRewardResult.amount);
            return rewardResult;
        }
    }
}
