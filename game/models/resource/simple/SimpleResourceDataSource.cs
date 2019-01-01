using common;
using logic.core.context;
using logic.core.reference.description;

namespace Assets.game.models.resource.simple
{
    public class SimpleResourceDataSource : DataSourceBase<SimpleResourceDescription>
    {
        public SimpleResourceDataSource(RawNode node, IContext context) : base(node, context)
        {
        }

        protected override SimpleResourceDescription Factory(RawNode node)
        {
            return new SimpleResourceDescription(node, GetContext());
        }
    }
}
