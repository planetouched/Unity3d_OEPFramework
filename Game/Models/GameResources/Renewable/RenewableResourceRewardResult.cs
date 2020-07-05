using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Util;
using Basement.BLFramework.Essential.Path;
using Basement.BLFramework.Essential.Rewards.Result;
using Basement.Common;

namespace Game.Models.GameResources.Renewable
{
    public class RenewableResourceRewardResult : RewardResult
    {
        public RenewableResource Resource { get; }
        public int Amount { get; }

        public RenewableResourceRewardResult(string type, RenewableResource resource, int amount, IContext context)
            : base(type, context)
        {
            this.Resource = resource;
            this.Amount = amount;
        }

        public RenewableResourceRewardResult(RawNode node, IContext context) : base(node, context)
        {
            Resource = PathUtil.GetModelPath(context, node.GetString("path"), null).GetSelf<RenewableResource>();
            Amount = node.GetInt("amount");
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict().SetArgs("type", type, "path", Resource.description.GetDescriptionPath(), "amount", Amount);
        }
    }
}