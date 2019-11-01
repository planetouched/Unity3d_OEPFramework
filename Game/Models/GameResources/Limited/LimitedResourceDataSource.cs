using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Game.Models.GameResources.Limited
{
    public class LimitedResourceDataSource : DataSourceBase<LimitedResourceDescription>
    {
        public LimitedResourceDataSource(RawNode node, IContext context = null) : base(node, context)
        {
        }

        protected override LimitedResourceDescription Factory(RawNode partialNode)
        {
            return new LimitedResourceDescription(partialNode, GetContext());
        }
    }
}
