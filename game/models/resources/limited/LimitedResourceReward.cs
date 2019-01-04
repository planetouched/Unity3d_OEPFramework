﻿using common;
using logic.core.context;
using logic.core.factories;
using logic.essential.amount;
using logic.essential.reward;
using logic.essential.reward.result;

namespace game.models.resources.limited
{
    public class LimitedResourceReward : Reward
    {
        private readonly IAmount _amount;

        public LimitedResourceReward(RawNode node, IContext context = null) : base(node, context)
        {
            _amount = FactoryManager.Build<Amount>(node.GetNode("amount"), context);
        }

        public override IRewardResult Calculate()
        {
            var path = RewardPath();

            if (path == null)
                return new RewardResult();

            var resource = path.GetSelf<LimitedResource>();
            int value = _amount.Number();
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
