using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Game.Models.Deals
{
    public class DealDataSource : DataSourceBase<DealDescription>
    {
        public DealDataSource(RawNode node, IContext context = null) : base(node, context)
        {
        }

        protected override DealDescription Factory(RawNode partialNode)
        {
            return new DealDescription(partialNode, GetContext());
        }
    }
}
