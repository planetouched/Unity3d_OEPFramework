using common;
using logic.core.context;
using logic.core.reference.dataSource;

namespace game.model.resource.limited
{
    public class LimitedResourceDataSource : DataSourceSelectableBase<LimitedResourceDescription>
    {
        public LimitedResourceDataSource(RawNode node, IContext context = null) : base(node, context)
        {
        }

        protected override LimitedResourceDescription Factory(RawNode node)
        {
            return new LimitedResourceDescription(node, GetContext());
        }
    }
}
