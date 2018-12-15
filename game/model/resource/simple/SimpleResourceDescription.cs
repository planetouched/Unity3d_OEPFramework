using common;
using logic.core.context;
using logic.core.reference.description;

namespace game.model.resource.simple
{
    public class SimpleResourceDescription : SelectableDescriptionBase
    {
        public SimpleResourceDescription(RawNode node, IContext context = null) : base(node, context)
        {
        }
    }
}
