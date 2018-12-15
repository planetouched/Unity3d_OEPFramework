using common;
using logic.core.context;
using logic.core.factories;

namespace logic.essential.reward
{
    public abstract class WrappedReward : Reward
    {
        public IReward innerReward { get; protected set; }

        protected WrappedReward(RawNode rawNode, IContext context)
            : base(rawNode, context)
        {
            innerReward = FactoryManager.Build<Reward>(rawNode.GetNode("reward"), context);
        }
    }
}
