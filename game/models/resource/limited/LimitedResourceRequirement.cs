using common;
using logic.core.context;
using logic.essential.requirement;

namespace Assets.game.models.resource.limited
{
    public class LimitedResourceRequirement : Requirement
    {
        public LimitedResource resource { get; private set; }
        public int amount { get; private set; }

        public LimitedResourceRequirement(RawNode node, IContext context) : base(node, context)
        {
            resource = GetPath().GetSelf<LimitedResource>();
            amount = node.GetInt("amount");
        }

        public override bool Check()
        {
            return resource.amount >= amount;
        }
    }
}
