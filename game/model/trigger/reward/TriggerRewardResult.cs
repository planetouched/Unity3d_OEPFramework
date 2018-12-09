using Assets.common;
using Assets.game.model.trigger._base;
using Assets.logic.core.context;
using Assets.logic.core.util;
using Assets.logic.essential.path;
using Assets.logic.essential.reward.result;

namespace Assets.game.model.trigger.reward
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
            trigger = PathUtil.ModelsPath(context, node.GetString("path"), null).GetSelf<Trigger>();
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict().SetArgs("path", PathUtil.StringPath(trigger));
        }
    }
}
