using common;
using logic.core.context;
using logic.core.reference.description;

namespace game.models.resources.limited
{
    public class LimitedResourceDataSource : DataSourceBase<LimitedResourceDescription>
    {
        public LimitedResourceDataSource(RawNode node, IContext context = null) : base(node, context)
        {
        }

        protected override LimitedResourceDescription Factory(RawNode partialNode)
        {
            return new LimitedResourceDescription(partialNode, GetContext());
        }
    }
}
