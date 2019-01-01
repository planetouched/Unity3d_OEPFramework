using common;
using logic.core.context;
using logic.core.reference.description;

namespace Assets.game.models.resource.renewable
{
    public class RenewableResourceDataSource : DataSourceBase<RenewableResourceDescription>
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
