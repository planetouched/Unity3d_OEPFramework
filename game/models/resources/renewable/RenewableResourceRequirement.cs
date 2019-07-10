using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Essential.Requirement;
using Basement.Common;

namespace Game.Models.Resources.Renewable
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
