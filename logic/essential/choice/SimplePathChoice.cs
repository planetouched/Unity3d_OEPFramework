using Assets.common;
using Assets.logic.core.context;
using Assets.logic.essential.path;

namespace Assets.logic.essential.choice
{
    public class SimplePathChoice : PathChoice
    {
        private readonly RawNode pathNode;

        public SimplePathChoice(RawNode node, IContext context)
            : base(node, context)
        {
            pathNode = node.CheckKey("path") ? node.GetNode("path") : node;
        }

        public override Path GetPath()
        {
            return Path.Create(GetContext(), pathNode);
        }
    }
}