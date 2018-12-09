using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.description;
using Assets.logic.core.throughEvent;
using Assets.logic.essential.path;

namespace Assets.logic.essential.choice
{
    public class PathChoice : DescriptionBase, IPathChoice
    {
        protected ModelsPath randomPath;

        public PathChoice(RawNode node, IContext context) : base(node, context)
        {
            if (node.CheckKey("random"))
                randomPath = PathUtil.ModelsPath(context, node.GetString("random"), null);
        }

        public virtual ModelsPath GetPath()
        {
            return null;
        }
    }
}
