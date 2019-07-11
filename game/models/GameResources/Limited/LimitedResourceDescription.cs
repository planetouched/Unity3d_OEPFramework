using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Game.Models.GameResources.Limited
{
    public class LimitedResourceDescription : DescriptionBase
    {
        public int maximum { get; private set; }

        public LimitedResourceDescription(RawNode node, IContext context = null) : base(node, context)
        {
            maximum = node.GetInt("max");
        }
    }
}
