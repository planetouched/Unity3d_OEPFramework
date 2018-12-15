using common;
using logic.core.context;
using logic.core.factories;
using logic.core.util;

namespace logic.essential.reward.result
{
    public class WrappedRewardResult : RewardResult
    {
        public IRewardResult rewardResult { get; private set; }

        public WrappedRewardResult(string type, IRewardResult rewardResult, IContext context = null)
            : base(type, context)
        {
            this.rewardResult = rewardResult;
        }

        public WrappedRewardResult(RawNode node, IContext context) : base(node, context)
        {
            rewardResult = FactoryManager.Build<RewardResult>(node.GetNode("result"), context);
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict().SetArgs("type", type, "result", rewardResult.Serialize());
        }
    }
}
