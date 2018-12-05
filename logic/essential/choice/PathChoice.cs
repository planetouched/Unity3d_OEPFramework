using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.description;
using Assets.logic.essential.path;

namespace Assets.logic.essential.choice
{
    public class PathChoice : DescriptionBase, IPathChoice
    {
        protected Path randomPath;
        protected RawNode node;

        public PathChoice(RawNode node, IContext context) : base(node, context)
        {
            this.node = node;

            if (node.CheckKey("random"))
                randomPath = Path.Create(context, node.GetString("random"), null);
        }

        public virtual Path GetPath()
        {
            return null;
        }
    }
}
