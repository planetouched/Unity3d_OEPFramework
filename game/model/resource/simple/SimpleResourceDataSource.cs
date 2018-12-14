using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.dataSource;

namespace Assets.game.model.resource.simple
{
    public class SimpleResourceDataSource : DataSourceSelectableBase<SimpleResourceDescription>
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
