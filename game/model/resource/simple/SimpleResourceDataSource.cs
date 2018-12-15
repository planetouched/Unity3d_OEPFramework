using common;
using logic.core.context;
using logic.core.reference.dataSource;

namespace game.model.resource.simple
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
