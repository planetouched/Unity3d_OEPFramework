using common;
using logic.core.context;
using logic.core.reference.description;
using logic.core.throughEvent;
using logic.essential.path;

namespace logic.essential.choice
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
