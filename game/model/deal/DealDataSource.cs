using common;
using logic.core.context;
using logic.core.reference.dataSource;

namespace game.model.deal
{
    public class DealDataSource : DataSourceSelectableBase<DealDescription>
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
