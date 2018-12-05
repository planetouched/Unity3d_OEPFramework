using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.factories;

namespace Assets.logic.essential.reward
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
