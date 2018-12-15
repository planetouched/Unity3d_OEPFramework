using common;
using logic.core.context;
using logic.core.throughEvent;
using logic.essential.path;

namespace logic.essential.choice
{
    public class SimplePathChoice : PathChoice
    {
        public SimplePathChoice(RawNode node, IContext context)
            : base(node, context)
        {
        }

        public override ModelsPath GetPath()
        {
            return PathUtil.ModelsPath(GetContext(), node.GetNode("path"));
        }
    }
}