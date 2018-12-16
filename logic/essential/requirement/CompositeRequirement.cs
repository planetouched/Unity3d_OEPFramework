using common;
using logic.core.context;
using logic.core.reference.collection;

namespace logic.essential.requirement
{
    public abstract class CompositeRequirement : Requirement
    {
        public LazyArray<Requirement> requirements { get; }

        protected CompositeRequirement(RawNode node, IContext context)
            : base(node, context)
        {
            requirements = new LazyArray<Requirement>(node.GetNode("requirements"), context);
        }
    }
}