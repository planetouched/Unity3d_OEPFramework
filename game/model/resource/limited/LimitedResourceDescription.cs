using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.description;

namespace Assets.game.model.resource.limited
{
    public class LimitedResourceDescription : SelectableDescriptionBase
    {
        public int maximum { get; private set; }

        public LimitedResourceDescription(RawNode node, IContext context = null) : base(node, context)
        {
            maximum = node.GetInt("max");
        }
    }
}
