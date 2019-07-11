using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Util;
using Basement.BLFramework.Essential.Path;
using Basement.BLFramework.Essential.Reward.Result;
using Basement.Common;

namespace Game.Models.GameResources.Simple
{
    public class SimpleResourceRewardResult : RewardResult
    {
        public SimpleResource resource { get; private set; }
        public int amount { get; private set; }

        public SimpleResourceRewardResult(string type, SimpleResource resource, int amount, IContext context)
            : base(type, context)
        {
            this.resource = resource;
            this.amount = amount;
        }

        public SimpleResourceRewardResult(RawNode node, IContext context) : base(node, context)
        {
            resource = PathUtil.GetModelPath(context, node.GetString("path"), null).GetSelf<SimpleResource>();
            amount = node.GetInt("amount");
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict().SetArgs("type", type, "path", resource.description.GetDescriptionPath(), "amount", amount);
        }
    }
}
