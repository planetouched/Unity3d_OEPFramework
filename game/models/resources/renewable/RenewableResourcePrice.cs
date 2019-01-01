using common;
using logic.core.context;
using logic.essential.price;

namespace game.models.resources.renewable
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
