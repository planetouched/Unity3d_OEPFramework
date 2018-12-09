using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.factories;
using Assets.logic.essential.amount;
using Assets.logic.essential.reward;
using Assets.logic.essential.reward.result;

namespace Assets.game.model.resource.renewableResource
{
    public class RenewableResourceReward : Reward
    {
        private readonly IAmount amount;

        public RenewableResourceReward(RawNode node, IContext context)
            : base(node, context)
        {
            amount = FactoryManager.Build<Amount>(node.GetNode("amount"), context);
        }

        public override IRewardResult Calculate()
        {
            var path = RewardPath();

            if (path == null)
            {
                return new RewardResult();
            }

            var renewableResource = path.GetSelf<RenewableResource>();
            int value = amount.Number();
            return new RenewableResourceRewardResult(type, renewableResource, value, GetContext());
        }

        public override IRewardResult Award(IRewardResult rewardResult)
        {
            var simpleResourceRewardResult = (RenewableResourceRewardResult)rewardResult;
            simpleResourceRewardResult.resource.Increment(simpleResourceRewardResult.amount);
            return rewardResult;
        }
    }
}