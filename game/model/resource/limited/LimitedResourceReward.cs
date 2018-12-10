using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.factories;
using Assets.logic.essential.amount;
using Assets.logic.essential.reward;
using Assets.logic.essential.reward.result;

namespace Assets.game.model.resource.limited
{
    public class LimitedResourceReward : Reward
    {
        private readonly IAmount amount;

        public LimitedResourceReward(RawNode node, IContext context = null) : base(node, context)
        {
            amount = FactoryManager.Build<Amount>(node.GetNode("amount"), context);
        }

        public override IRewardResult Calculate()
        {
            var path = RewardPath();

            if (path == null)
                return new RewardResult();

            var resource = path.GetSelf<LimitedResource>();
            int value = amount.Number();
            return new LimitedResourceRewardResult(type, resource, value, GetContext());
        }

        public override IRewardResult Award(IRewardResult rewardResult)
        {
            var simpleResourceRewardResult = (LimitedResourceRewardResult)rewardResult;
            simpleResourceRewardResult.resource.Change(simpleResourceRewardResult.amount);
            return rewardResult;
        }
    }
}
