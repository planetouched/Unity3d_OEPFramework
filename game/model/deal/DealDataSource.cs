using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.dataSource;

namespace Assets.game.model.deal
{
    public class DealDataSource : DataSourceDescriptionBase<DealDescription>
    {
        public DealDataSource(RawNode node, IContext context = null) : base(node, context)
        {
        }

        protected override DealDescription Factory(RawNode node)
        {
            return new DealDescription(node, GetContext());
        }
    }
}
