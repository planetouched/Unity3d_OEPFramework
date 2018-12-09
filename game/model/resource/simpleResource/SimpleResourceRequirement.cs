using Assets.common;
using Assets.logic.core.context;
using Assets.logic.essential.requirement;

namespace Assets.game.model.resource.simpleResource
{
    public class SimpleResourceRequirement : Requirement
    {
        public SimpleResource resource { get; private set; }
        public int amount { get; private set; }

        public SimpleResourceRequirement(RawNode node, IContext context) : base(node, context)
        {
            resource = GetPath().GetSelf<SimpleResource>();
            amount = node.GetInt("amount");
        }

        public override bool Check()
        {
            return resource.amount >= amount;
        }
    }
}
