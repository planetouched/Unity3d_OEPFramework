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

        public override ModelsPath GetModelPath()
        {
            return PathUtil.GetModelPath(GetContext(), node.GetString("path"), random);
        }

        public override T GetDescription<T>()
        {
            return PathUtil.GetDescription<T>(GetContext(), node.GetString("path"), random);
        }
    }
}