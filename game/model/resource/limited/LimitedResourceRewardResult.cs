using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.util;
using Assets.logic.essential.path;
using Assets.logic.essential.reward.result;

namespace Assets.game.model.resource.limited
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
            resource = PathUtil.ModelsPath(context, node.GetString("path"), null).GetSelf<LimitedResource>();
            amount = node.GetInt("amount");
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict().SetArgs("type", type, "path", PathUtil.StringPath(resource), "amount", amount);
        }
    }
}
