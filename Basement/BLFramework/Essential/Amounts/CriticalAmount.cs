using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Essential.Path;
using Basement.Common;

namespace Basement.BLFramework.Essential.Amount
{
    public class CriticalAmount : Amount
    {
        public double probability { get; }
        public int critical { get; }
        public int regular { get; }
        public bool wasCritical { get; private set; }
        private readonly Random.Random _random;

        public CriticalAmount(RawNode node, IContext context)
            : base(node, context)
        {
            probability = node.GetDouble("probability");
            critical = node.GetInt("critical");
            regular = node.GetInt("regular");
            _random = PathUtil.GetModelPath(GetContext(), node.GetString("random"), null).GetSelf<Random.Random>();
        }

        public override int Number()
        {
            wasCritical = _random.NextDouble() <= probability;
            return wasCritical ? critical : regular;
        }
    }
}