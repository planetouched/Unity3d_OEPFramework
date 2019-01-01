using common;
using logic.core.context;
using logic.core.reference.description;

namespace game.models.resources.renewable
{
    public class RenewableResourceDataSource : DataSourceBase<RenewableResourceDescription>
    {
        public RenewableResourceDataSource(RawNode node, IContext context) : base(node, context)
        {
        }

        protected override RenewableResourceDescription Factory(RawNode partialNode)
        {
            return new RenewableResourceDescription(partialNode, GetContext());
        }
    }
}
