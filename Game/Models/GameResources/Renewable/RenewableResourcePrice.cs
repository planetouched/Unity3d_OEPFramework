using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Essential.Prices;
using Basement.Common;

namespace Game.Models.GameResources.Renewable
{
    public class RenewableResourcePrice : Price
    {
        public readonly RenewableResource _resource;
        
        public RenewableResourcePrice(RawNode rawNode, IContext context) : base(rawNode, context)
        {
            _resource = GetPath().GetSelf<RenewableResource>();
        }

        public override bool Check()
        {
            return _resource.Amount >= amount;
        }

        public override void Pay()
        {
            if (!Check()) return;
            _resource.Change(-amount);
        }
    }
}
