using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Essential.Price;
using Basement.Common;

namespace Game.Models.Resources.Renewable
{
    public class RenewableResourcePrice : Price
    {
        public RenewableResourcePrice(RawNode rawNode, IContext context) : base(rawNode, context)
        {
        }

        public override bool Check()
        {
            return GetPath().GetSelf<RenewableResource>().amount >= amount;
        }

        public override void Pay()
        {
            if (!Check()) return;
            GetPath().GetSelf<RenewableResource>().Change(-amount);
        }
    }
}
