using Assets.common;
using Assets.logic.core.context;
using Assets.logic.essential.requirement;

namespace Assets.game.model.resource.renewableResource
{
    public class RenewableResourceRequirement : Requirement
    {
        public RenewableResource renewableResource { get; private set; }
        public int amount { get; private set; }

        public RenewableResourceRequirement(RawNode node, IContext context) : base(node, context)
        {
            renewableResource = GetPath().result.GetSelf<RenewableResource>();
            amount = node.GetInt("amount");
        }

        public override bool Check()
        {
            var res = GetPath().result.GetSelf<RenewableResource>();
            return res.amount >= amount;
        }
    }
}
