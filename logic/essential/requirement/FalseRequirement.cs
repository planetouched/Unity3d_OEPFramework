using common;
using logic.core.context;

namespace logic.essential.requirement
{
    public class FalseRequirement : Requirement
    {
        public FalseRequirement(RawNode node, IContext context) 
            : base(node, context)
        {
        }

        public override bool Check()
        {
            return false;
        }
    }
}