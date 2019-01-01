using common;
using logic.core.context;
using logic.core.reference.description;

namespace game.models.resources.simple
{
    public class SimpleResourceDataSource : DataSourceBase<SimpleResourceDescription>
    {
        public SimpleResourceDataSource(RawNode node, IContext context) : base(node, context)
        {
        }

        protected override SimpleResourceDescription Factory(RawNode partialNode)
        {
            return new SimpleResourceDescription(partialNode, GetContext());
        }
    }
}
