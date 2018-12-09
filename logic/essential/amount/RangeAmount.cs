using Assets.common;
using Assets.logic.core.context;
using Assets.logic.essential.path;
using Assets.logic.essential.random;

namespace Assets.logic.essential.amount
{
    public class RangeAmount : Amount
    {
        public int min { get; private set; }
        public int max { get; private set; }
        private readonly Random random;

        public RangeAmount(RawNode node, IContext context)
            : base(node, context)
        {
            random = PathUtil.ModelsPath(GetContext(), node.GetString("random"), null).GetSelf<Random>();
            min = node.GetInt("min");
            max = node.GetInt("max");
        }

        public override int Number()
        {
            return random.Range(min, max + 1);
        }
    }
}