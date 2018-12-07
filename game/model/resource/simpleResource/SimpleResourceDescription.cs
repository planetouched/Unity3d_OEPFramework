using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.description;

namespace Assets.game.model.resource.simpleResource
{
    public class SimpleResourceDescription : SelectableDescriptionBase
    {
        public SimpleResourceDescription(RawNode node, IContext context = null) : base(node, context)
        {
        }
    }
}
