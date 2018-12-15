using common;
using logic.core.context;

namespace logic.essential.amount
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