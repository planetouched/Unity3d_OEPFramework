using Assets.common;
using Assets.game.model.triggers._base;
using Assets.logic.core.context;
using Assets.logic.core.util;
using Assets.logic.essential.path;
using Assets.logic.essential.reward.result;

namespace Assets.game.model.triggers.reward
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
            trigger = Path.Create(context, node.GetString("path"), null).result.GetSelf<Trigger>();
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict().SetArgs("path", Path.StringPath(trigger));
        }
    }
}
