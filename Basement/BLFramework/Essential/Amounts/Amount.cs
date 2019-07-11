using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Basement.BLFramework.Essential.Amount
{
    public class Amount : DescriptionBase, IAmount
    {
        public string type { get; }

        public Amount(RawNode node, IContext context) : base(node, context)
        {
            type = node.GetString("type");
        }

        public virtual int Number()
        {
            return 0;
        }
    }
}