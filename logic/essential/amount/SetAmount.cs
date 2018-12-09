using Assets.common;
using Assets.logic.core.context;
using Assets.logic.essential.path;
using Assets.logic.essential.random;

namespace Assets.logic.essential.amount
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