using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.throughEvent;
using Assets.logic.essential.path;

namespace Assets.logic.essential.choice
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