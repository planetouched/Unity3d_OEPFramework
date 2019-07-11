using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Util;
using Basement.BLFramework.Essential.Path;
using Basement.BLFramework.Essential.Rewards.Result;
using Basement.Common;
using Game.Models.Triggers._Base;

namespace Game.Models.Triggers.Reward
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
            return SerializeUtil.Dict().SetArgs("path", trigger.description.GetDescriptionPath());
        }
    }
}
