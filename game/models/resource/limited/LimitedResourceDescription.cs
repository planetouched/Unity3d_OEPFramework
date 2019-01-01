using common;
using logic.core.context;
using logic.core.reference.description;

namespace Assets.game.models.resource.limited
{
    public class LimitedResourceDescription : DescriptionBase
    {
        public int maximum { get; private set; }

        public LimitedResourceDescription(RawNode node, IContext context = null) : base(node, context)
        {
            maximum = node.GetInt("max");
        }
    }
}
