using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Essential.Prices;
using Basement.Common;

namespace Game.Models.GameResources.Limited
{
    public class LimitedResourcePrice : Price
    {
        public LimitedResource resource { get; private set; }

        public LimitedResourcePrice(RawNode node, IContext context) : base(node, context)
        {
            resource = GetPath().GetSelf<LimitedResource>();
        }

        public override bool Check()
        {
            return resource.amount >= amount;
        }

        public override void Pay()
        {
            if (!Check()) return;
            resource.Change(-amount);
        }
    }
}
