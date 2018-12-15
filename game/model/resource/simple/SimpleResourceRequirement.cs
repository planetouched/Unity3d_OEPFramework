using common;
using logic.core.context;
using logic.essential.requirement;

namespace game.model.resource.simple
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
