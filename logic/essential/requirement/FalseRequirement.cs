using Assets.common;
using Assets.logic.core.context;

namespace Assets.logic.essential.requirement
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