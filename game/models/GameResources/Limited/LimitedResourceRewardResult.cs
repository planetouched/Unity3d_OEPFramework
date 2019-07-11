using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Util;
using Basement.BLFramework.Essential.Path;
using Basement.BLFramework.Essential.Reward.Result;
using Basement.Common;

namespace Game.Models.GameResources.Limited
{
    public class LimitedResourceRewardResult : RewardResult
    {
        public LimitedResource resource { get; private set; }
        public int amount { get; private set; }

        public LimitedResourceRewardResult(string type, LimitedResource resource, int amount, IContext context)
            : base(type, context)
        {
            this.resource = resource;
            this.amount = amount;
        }

        public LimitedResourceRewardResult(RawNode node, IContext context) : base(node, context)
        {
            resource = PathUtil.GetModelPath(context, node.GetString("path"), null).GetSelf<LimitedResource>();
            amount = node.GetInt("amount");
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict().SetArgs("type", type, "path", resource.description.GetDescriptionPath(), "amount", amount);
        }
    }
}
