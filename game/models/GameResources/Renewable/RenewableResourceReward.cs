using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Factories;
using Basement.BLFramework.Essential.Amount;
using Basement.BLFramework.Essential.Reward;
using Basement.BLFramework.Essential.Reward.Result;
using Basement.Common;

namespace Game.Models.GameResources.Renewable
{
    public class RenewableResourceReward : Reward
    {
        private readonly IAmount _amount;

        public RenewableResourceReward(RawNode node, IContext context)
            : base(node, context)
        {
            _amount = FactoryManager.Build<Amount>(node.GetNode("amount"), context);
        }

        public override IRewardResult Calculate()
        {
            var path = RewardPath();

            if (path == null)
            {
                return new RewardResult();
            }

            var resource = path.GetSelf<RenewableResource>();
            int value = _amount.Number();
            return new RenewableResourceRewardResult(type, resource, value, GetContext());
        }

        public override IRewardResult Award(IRewardResult rewardResult)
        {
            var simpleResourceRewardResult = (RenewableResourceRewardResult)rewardResult;
            simpleResourceRewardResult.resource.Change(simpleResourceRewardResult.amount);
            return rewardResult;
        }
    }
}