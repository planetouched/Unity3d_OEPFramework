using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.collection;

namespace Assets.logic.essential.price
{
    public class CompositePrice : Price
    {
        public LazyArray<Price> prices { get; private set; }

        public CompositePrice(RawNode rawNode, IContext context)
            : base(rawNode, context)
        {
            prices = new LazyArray<Price>(rawNode.GetNode("prices"), context);
        }

        public override bool Check()
        {
            foreach (var price in prices)
            {
                if (!price.Value.Check())
                    return false;
            }
            return true;
        }

        public override void Pay()
        {
            foreach (var price in prices)
                price.Value.Pay();
        }
    }
}
