using common;
using logic.core.context;
using logic.essential.path;
using logic.essential.random;

namespace logic.essential.amount
{
    public class CriticalAmount : Amount
    {
        public double probability { get; }
        public int critical { get; }
        public int regular { get; }
        public bool wasCritical { get; private set; }
        private readonly Random random;

        public CriticalAmount(RawNode node, IContext context)
            : base(node, context)
        {
            probability = node.GetDouble("probability");
            critical = node.GetInt("critical");
            regular = node.GetInt("regular");
            random = PathUtil.GetModelPath(GetContext(), node.GetString("random"), null).GetSelf<Random>();
        }

        public override int Number()
        {
            wasCritical = random.NextDouble() <= probability;
            return wasCritical ? critical : regular;
        }
    }
}