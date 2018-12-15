using common;
using logic.core.context;
using logic.core.reference.dataSource;

namespace game.model.trigger
{
    public class TriggerDataSource : DataSourceSelectableBase<TriggerDescription>
    {
        public TriggerDataSource(RawNode node, IContext context = null) : base(node, context)
        {
        }

        protected override TriggerDescription Factory(RawNode node)
        {
            return new TriggerDescription(node, GetContext());
        }
    }
}