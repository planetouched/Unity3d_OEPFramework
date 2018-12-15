using common;
using logic.core.context;

namespace logic.essential.reward.result
{
    public class RewardResult : IRewardResult
    {
        public string type { get; private set; }
        private readonly WeakRef<IContext> weakContext;

        public RewardResult()
        {
        }

        public RewardResult(string type, IContext context = null)
        {
            this.type = type;

            if (context != null)
            {
                weakContext = new WeakRef<IContext>(context);
            }
        }

        public RewardResult(RawNode node, IContext context)
        {
            type = node.GetString("type");
            weakContext = new WeakRef<IContext>(context);
        }

        public virtual object Serialize()
        {
            return null;
        }

        public IContext GetContext()
        {
            if (weakContext != null)
            {
                return weakContext.obj;
            }

            return null;
        }
    }
}
