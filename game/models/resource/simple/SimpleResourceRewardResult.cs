using common;
using logic.core.context;
using logic.core.util;
using logic.essential.path;
using logic.essential.reward.result;

namespace Assets.game.models.resource.simple
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
            return SerializeUtil.Dict().SetArgs("type", type, "path", PathUtil.GetStringPath(resource), "amount", amount);
        }
    }
}
