using Assets.game.models.trigger._base;
using common;
using logic.core.context;
using logic.essential.reward;
using logic.essential.reward.result;

namespace Assets.game.models.trigger.reward
{
    public class ActivateTriggerReward : Reward
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