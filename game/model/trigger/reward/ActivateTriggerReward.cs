using Assets.common;
using Assets.game.model.trigger._base;
using Assets.logic.core.context;
using Assets.logic.essential.reward;
using Assets.logic.essential.reward.result;

namespace Assets.game.model.trigger.reward
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