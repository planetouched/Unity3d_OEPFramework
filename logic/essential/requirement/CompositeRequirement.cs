using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.collection;

namespace Assets.logic.essential.requirement
{
    public abstract class CompositeRequirement : Requirement
    {
        public LazyArray<Requirement> requirements { get; protected set; }

        protected CompositeRequirement(RawNode node, IContext context)
            : base(node, context)
        {
            requirements = new LazyArray<Requirement>(node.GetNode("requirements"), context);
        }
    }
}