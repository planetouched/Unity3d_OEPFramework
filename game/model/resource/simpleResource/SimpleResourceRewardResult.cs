using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.util;
using Assets.logic.essential.path;
using Assets.logic.essential.reward.result;

namespace Assets.game.model.resource.simpleResource
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
            resource = Path.Create(context, node.GetString("path"), null).result.GetSelf<SimpleResource>();
            amount = node.GetInt("amount");
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict("type", type, "path", Path.GetPath(resource), "amount", amount);
        }
    }
}
