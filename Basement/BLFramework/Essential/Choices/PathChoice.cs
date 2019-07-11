using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.BLFramework.Core.ThroughEvent;
using Basement.BLFramework.Essential.Path;
using Basement.BLFramework.Essential.Random;
using Basement.Common;

namespace Basement.BLFramework.Essential.Choice
{
    public class PathChoice : DescriptionBase, IPathChoice
    {
        protected readonly IRandom random;

        public PathChoice(RawNode node, IContext context) : base(node, context)
        {
            if (node.CheckKey("random"))
            {
                random = PathUtil.GetModelPath(context, node.GetString("random"), null).GetSelf<Random.Random>();
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
