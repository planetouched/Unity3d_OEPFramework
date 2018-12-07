using Assets.common;
using Assets.logic.core.context;
using Assets.logic.essential.price;

namespace Assets.game.model.resource.renewableResource
{
    public class RenewableResourcePrice : Price
    {
        public RenewableResourcePrice(RawNode rawNode, IContext context) : base(rawNode, context)
        {
        }

        public override bool Check()
        {
            return GetPath().result.GetSelf<RenewableResource>().amount >= amount;
        }

        public override void Pay()
        {
            if (!Check()) return;
            GetPath().result.GetSelf<RenewableResource>().Increment(-amount);
        }
    }
}
