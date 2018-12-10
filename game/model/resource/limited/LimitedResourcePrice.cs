using Assets.common;
using Assets.logic.core.context;
using Assets.logic.essential.price;

namespace Assets.game.model.resource.limited
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
