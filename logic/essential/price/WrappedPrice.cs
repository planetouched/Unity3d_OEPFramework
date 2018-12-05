using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.factories;

namespace Assets.logic.essential.price
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
