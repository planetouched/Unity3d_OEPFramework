using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.util;
using Assets.logic.essential.path;
using Assets.logic.essential.reward.result;

namespace Assets.game.model.resource.renewableResource
{
    internal class RenewableResourceRewardResult : RewardResult
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
            resource = Path.Create(context, node.GetString("path"), null).result.GetSelf<RenewableResource>();
            amount = node.GetInt("amount");
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict("type", type, "path", Path.GetPath(resource), "amount", amount);
        }
    }
}