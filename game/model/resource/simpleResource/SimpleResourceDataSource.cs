using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.dataSource;

namespace Assets.game.model.resource.simpleResource
{
    public class SimpleResourceDataSource : DataSourceDescriptionBase<SimpleResourceDescription>
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
