using common;
using logic.core.context;
using logic.core.reference.description;

namespace game.models.triggers
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