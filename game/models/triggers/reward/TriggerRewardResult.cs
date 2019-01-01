using Assets.game.models.triggers._base;
using common;
using logic.core.context;
using logic.core.util;
using logic.essential.path;
using logic.essential.reward.result;

namespace Assets.game.models.triggers.reward
{
    public class TriggerRewardResult : RewardResult
    {
        public Trigger trigger { get; private set; }

        public TriggerRewardResult(string type, Trigger trigger) : base(type)
        {
            this.trigger = trigger;
        }

        public TriggerRewardResult(RawNode node, IContext context) : base(node, context)
        {
            trigger = PathUtil.GetModelPath(context, node.GetString("path"), null).GetSelf<Trigger>();
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict().SetArgs("path", PathUtil.GetStringPath(trigger));
        }
    }
}
