using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Essential.Rewards.Result;
using Basement.Common;
using Game.Models.Triggers._Base;

namespace Game.Models.Triggers.Reward
{
    public class ActivateTriggerReward : Basement.BLFramework.Essential.Rewards.Reward
    {
        public ActivateTriggerReward(RawNode rawNode, IContext context) 
            : base(rawNode, context)
        {
        }

        public override IRewardResult Calculate()
        {
            var trigger = RewardPath().GetSelf<Trigger>();
            return new TriggerRewardResult(type, trigger);
        }

        public override IRewardResult Award(IRewardResult rewardResult)
        {
            var result = (TriggerRewardResult) rewardResult;
            result.trigger.Activate();
            return result;
        }
    }
}