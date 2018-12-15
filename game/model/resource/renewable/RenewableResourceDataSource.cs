using common;
using logic.core.context;
using logic.core.reference.dataSource;

namespace game.model.resource.renewable
{
    public class RenewableResourceDataSource : DataSourceSelectableBase<RenewableResourceDescription>
    {
        public RenewableResourceDataSource(RawNode node, IContext context) : base(node, context)
        {
        }

        protected override RenewableResourceDescription Factory(RawNode node)
        {
            return new RenewableResourceDescription(node, GetContext());
        }
    }
}
