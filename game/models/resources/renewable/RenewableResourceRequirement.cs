using common;
using logic.core.context;
using logic.essential.requirement;

namespace Assets.game.models.resources.renewable
{
    public class RenewableResourceRequirement : Requirement
    {
        public RenewableResource renewableResource { get; private set; }
        public int amount { get; private set; }

        public RenewableResourceRequirement(RawNode node, IContext context) : base(node, context)
        {
            renewableResource = GetPath().GetSelf<RenewableResource>();
            amount = node.GetInt("amount");
        }

        public override bool Check()
        {
            var res = GetPath().GetSelf<RenewableResource>();
            return res.amount >= amount;
        }
    }
}
