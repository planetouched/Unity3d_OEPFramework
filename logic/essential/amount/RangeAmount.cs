using common;
using logic.core.context;
using logic.essential.path;
using logic.essential.random;

namespace logic.essential.amount
{
    public class RangeAmount : Amount
    {
        public int min { get; }
        public int max { get; }
        private readonly Random random;

        public RangeAmount(RawNode node, IContext context)
            : base(node, context)
        {
            random = PathUtil.GetModelPath(GetContext(), node.GetString("random"), null).GetSelf<Random>();
            min = node.GetInt("min");
            max = node.GetInt("max");
        }

        public override int Number()
        {
            return random.Range(min, max + 1);
        }
    }
}