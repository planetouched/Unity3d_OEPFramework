using common;
using logic.core.context;
using logic.core.reference.description;

namespace game.model.resource.limited
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
