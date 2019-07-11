using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Essential.Path;
using Basement.Common;

namespace Basement.BLFramework.Essential.Amount
{
    public class RangeAmount : Amount
    {
        public int min { get; }
        public int max { get; }
        private readonly Random.Random _random;

        public RangeAmount(RawNode node, IContext context)
            : base(node, context)
        {
            _random = PathUtil.GetModelPath(GetContext(), node.GetString("random"), null).GetSelf<Random.Random>();
            min = node.GetInt("min");
            max = node.GetInt("max");
        }

        public override int Number()
        {
            return _random.Range(min, max + 1);
        }
    }
}