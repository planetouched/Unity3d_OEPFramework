using common;
using logic.core.context;
using logic.core.factories;

namespace logic.essential.requirement
{
    public abstract class WrappedRequirement : Requirement
    {
        protected IRequirement innerRequirement;

        protected WrappedRequirement(RawNode node, IContext context) : base(node, context)
        {
            innerRequirement = FactoryManager.Build<Requirement>(node.GetNode("requirement"), context);
        }
    }
}
