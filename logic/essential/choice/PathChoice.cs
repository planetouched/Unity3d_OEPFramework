using common;
using logic.core.context;
using logic.core.reference.description;
using logic.core.throughEvent;
using logic.essential.path;
using logic.essential.random;

namespace logic.essential.choice
{
    public class PathChoice : DescriptionBase, IPathChoice
    {
        protected readonly IRandom random;

        public PathChoice(RawNode node, IContext context) : base(node, context)
        {
            if (node.CheckKey("random"))
            {
                random = PathUtil.GetModelPath(context, node.GetString("random"), null).GetSelf<Random>();
            }
        }

        public virtual ModelsPath GetModelPath()
        {
            return null;
        }

        public virtual T GetDescription<T>() where T : class, IDescription
        {
            return null;
        }
    }
}
