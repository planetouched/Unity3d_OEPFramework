using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Util;
using Basement.BLFramework.Essential.Path;
using Basement.BLFramework.Essential.Rewards.Result;
using Basement.Common;

namespace Game.Models.GameResources.Renewable
{
    public class RenewableResourceRewardResult : RewardResult
    {
        public RenewableResource resource { get; private set; }
        public int amount { get; private set; }

        public RenewableResourceRewardResult(string type, RenewableResource resource, int amount, IContext context)
            : base(type, context)
        {
            this.resource = resource;
            this.amount = amount;
        }

        public RenewableResourceRewardResult(RawNode node, IContext context) : base(node, context)
        {
            resource = PathUtil.GetModelPath(context, node.GetString("path"), null).GetSelf<RenewableResource>();
            amount = node.GetInt("amount");
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict().SetArgs("type", type, "path", resource.description.GetDescriptionPath(), "amount", amount);
        }
    }
}