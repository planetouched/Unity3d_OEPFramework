using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Essential.Requirements;
using Basement.Common;

namespace Game.Models.GameResources.Renewable
{
    public class RenewableResourceRequirement : Requirement
    {
        private readonly RenewableResource _renewableResource;
        private readonly int _amount;

        public RenewableResourceRequirement(RawNode node, IContext context) : base(node, context)
        {
            _renewableResource = GetPath().GetSelf<RenewableResource>();
            _amount = node.GetInt("amount");
        }

        public override bool Check()
        {
            var res = GetPath().GetSelf<RenewableResource>();
            return res.Amount >= _amount;
        }
    }
}
