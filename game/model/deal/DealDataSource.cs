using common;
using logic.core.context;
using logic.core.reference.description;

namespace game.model.deal
{
    public class DealDataSource : DataSourceBase<DealDescription>
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
