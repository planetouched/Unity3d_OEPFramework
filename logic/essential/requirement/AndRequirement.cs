using Assets.common;
using Assets.logic.core.context;

namespace Assets.logic.essential.requirement
{
    public class AndRequirement : CompositeRequirement
    {
        public AndRequirement(RawNode node, IContext context)
            : base(node, context)
        {
        }

        public override bool Check()
        {
            foreach (var requirement in requirements)
            {
                if (!requirement.Value.Check()) return false;
            }

            return true;
        }
    }
}