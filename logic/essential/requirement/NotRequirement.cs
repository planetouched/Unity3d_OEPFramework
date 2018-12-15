using common;
using logic.core.context;

namespace logic.essential.requirement
{
    public class NotRequirement : WrappedRequirement
    {
        public NotRequirement(RawNode node, IContext context) : base(node, context)
        {
        }

        public override bool Check()
        {
            return !innerRequirement.Check();
        }
    }
}
