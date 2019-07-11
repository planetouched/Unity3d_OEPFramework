using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Factories;
using Basement.BLFramework.Essential.Amounts;
using Basement.BLFramework.Essential.Rewards;
using Basement.BLFramework.Essential.Rewards.Result;
using Basement.Common;

namespace Game.Models.GameResources.Simple
{
    public class SimpleResourceReward : Reward
    {
        private readonly IAmount _amount;

        public SimpleResourceReward(RawNode node, IContext context = null) : base(node, context)
        {
            _amount = FactoryManager.Build<Amount>(node.GetNode("amount"), context);
        }

        public override IRewardResult Calculate()
        {
            var path = RewardPath();

            if (path == null)
                return new RewardResult();

            var resource = path.GetSelf<SimpleResource>();
            int value = _amount.Number();
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
