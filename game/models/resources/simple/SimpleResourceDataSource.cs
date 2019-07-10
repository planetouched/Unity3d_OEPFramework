using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Game.Models.Resources.Simple
{
    public class SimpleResourceDataSource : DataSourceBase<SimpleResourceDescription>
    {
        public SimpleResourceDataSource(RawNode node, IContext context) : base(node, context)
        {
        }

        protected override SimpleResourceDescription Factory(RawNode partialNode)
        {
            return new SimpleResourceDescription(partialNode, GetContext());
        }
    }
}
