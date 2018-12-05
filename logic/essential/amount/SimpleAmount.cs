using Assets.common;
using Assets.logic.core.context;

namespace Assets.logic.essential.amount
{
    public class SimpleAmount : Amount
    {
        public int amount { get; private set; }

        public SimpleAmount(RawNode node, IContext context)
            : base(node, context)
        {
            amount = node.GetInt("amount");
        }
        public override int Number()
        {
            return amount;
        }
    }
}