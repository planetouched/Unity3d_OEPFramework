using common;
using logic.core.context;

namespace logic.essential.reward.result
{
    public class RewardResult : IRewardResult
    {
        public string type { get; }
        private readonly WeakRef<IContext> _weakContext;

        public RewardResult()
        {
        }

        public RewardResult(string type, IContext context = null)
        {
            this.type = type;

            if (context != null)
            {
                _weakContext = new WeakRef<IContext>(context);
            }
        }

        public RewardResult(RawNode node, IContext context)
        {
            type = node.GetString("type");
            _weakContext = new WeakRef<IContext>(context);
        }

        public virtual object Serialize()
        {
            return null;
        }

        public IContext GetContext()
        {
            if (_weakContext != null)
            {
                return _weakContext.obj;
            }

            return null;
        }
    }
}
