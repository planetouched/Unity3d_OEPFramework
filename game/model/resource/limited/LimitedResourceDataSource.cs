using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.dataSource;

namespace Assets.game.model.resource.limited
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
