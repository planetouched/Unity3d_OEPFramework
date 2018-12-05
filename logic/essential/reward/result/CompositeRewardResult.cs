using System.Collections.Generic;

namespace Assets.logic.essential.reward.result
{
    public class CompositeRewardResult : RewardResult
    {
        public IEnumerable<IRewardResult> results { get; private set; }

        public CompositeRewardResult(string type, IEnumerable<IRewardResult> results)
            : base(type)
        {
            this.results = results;
        }
    }
}