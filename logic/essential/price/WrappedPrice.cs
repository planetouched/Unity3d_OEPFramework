using common;
using logic.core.context;
using logic.core.factories;

namespace logic.essential.price
{
    public abstract class WrappedPrice : Price
    {
        protected IPrice innerPrice;

        protected WrappedPrice(RawNode node, IContext context) : base(node, context)
        {
            innerPrice = FactoryManager.Build<Price>(node.GetNode("price"), context);
        }
    }
}
