using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.dataSource;

namespace Assets.game.model.resource.renewable
{
    public class RenewableResourceDataSource : DataSourceDescriptionBase<RenewableResourceDescription>
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
