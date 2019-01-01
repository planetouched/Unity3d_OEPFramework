using common;
using logic.core.context;
using logic.core.reference.description;

namespace game.models.resources.simple
{
    public class SimpleResourceDescription : DescriptionBase
    {
        public SimpleResourceDescription(RawNode node, IContext context = null) : base(node, context)
        {
        }
    }
}
