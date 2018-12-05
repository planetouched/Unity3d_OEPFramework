using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.factories;

namespace Assets.logic.essential.requirement
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
