using common;
using logic.core.context;
using logic.essential.path;
using logic.essential.random;

namespace logic.essential.amount
{
    public class SetAmount : Amount
    {
        private readonly int[] elements;
        private readonly Random random;

        public SetAmount(RawNode node, IContext context)
            : base(node, context)
        {
            elements = node.GetIntArray("elements");
            random = PathUtil.ModelsPath(GetContext(), node.GetString("random"), null).GetSelf<Random>();
        }

        public override int Number()
        {
            return elements[random.Range(0, elements.Length)];
        }
    }
}