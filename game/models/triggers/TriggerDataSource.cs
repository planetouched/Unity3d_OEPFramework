using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Game.Models.Triggers
{
    public class TriggerDataSource : DataSourceBase<TriggerDescription>
    {
        public TriggerDataSource(RawNode node, IContext context = null) : base(node, context)
        {
        }

        protected override TriggerDescription Factory(RawNode partialNode)
        {
            return new TriggerDescription(partialNode, GetContext());
        }
    }
}