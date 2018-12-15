using common;
using logic.core.context;

namespace logic.essential.requirement
{
    public class OrRequirement : CompositeRequirement
    {
        public OrRequirement(RawNode node, IContext context)
            : base(node, context)
        {
        }

        public override bool Check()
        {
            foreach (var requirement in requirements)
            {
                if (requirement.Value.Check()) return true;
            }

            return false;
        }
    }
}