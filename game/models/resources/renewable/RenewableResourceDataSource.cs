using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Game.Models.Resources.Renewable
{
    public class RenewableResourceDataSource : DataSourceBase<RenewableResourceDescription>
    {
        public RenewableResourceDataSource(RawNode node, IContext context) : base(node, context)
        {
        }

        protected override RenewableResourceDescription Factory(RawNode partialNode)
        {
            return new RenewableResourceDescription(partialNode, GetContext());
        }
    }
}
