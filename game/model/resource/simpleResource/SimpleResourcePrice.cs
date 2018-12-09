using Assets.common;
using Assets.logic.core.context;
using Assets.logic.essential.price;

namespace Assets.game.model.resource.simpleResource
{
    public class SimpleResourcePrice : Price
    {
        public SimpleResource resource { get; private set; }

        public SimpleResourcePrice(RawNode node, IContext context) : base(node, context)
        {
            resource = GetPath().GetSelf<SimpleResource>();
        }

        public override bool Check()
        {
            return resource.amount >= amount;
        }

        public override void Pay()
        {
            if (!Check()) return;
            resource.Increment(-amount);
        }
    }
}
