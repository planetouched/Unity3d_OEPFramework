using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.dataSource;

namespace Assets.game.model.triggers
{
    public class TriggerDataSource : DataSourceDescriptionBase<TriggerDescription>
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