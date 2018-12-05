using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.description;

namespace Assets.logic.essential.random
{
    public class RandomDescription : SelectableDescriptionBase
    {
        public string type { get; private set; }

        public RandomDescription(RawNode node, IContext context = null) : base(node, context)
        {
            type = node.GetString("type");
        }
    }
}
