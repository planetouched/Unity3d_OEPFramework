using common;
using logic.core.context;
using logic.essential.path;
using logic.essential.random;

namespace logic.essential.amount
{
    public class SetAmount : Amount
    {
        private readonly int[] _elements;
        private readonly Random _random;

        public SetAmount(RawNode node, IContext context)
            : base(node, context)
        {
            _elements = node.GetIntArray("elements");
            _random = PathUtil.GetModelPath(GetContext(), node.GetString("random"), null).GetSelf<Random>();
        }

        public override int Number()
        {
            return _elements[_random.Range(0, _elements.Length)];
        }
    }
}