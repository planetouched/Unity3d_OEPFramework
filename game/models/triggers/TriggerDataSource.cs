using common;
using logic.core.context;
using logic.core.reference.description;

namespace Assets.game.models.triggers
{
    public class TriggerDataSource : DataSourceBase<TriggerDescription>
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