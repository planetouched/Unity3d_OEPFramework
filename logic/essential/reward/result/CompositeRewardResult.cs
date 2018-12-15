﻿using common;
using logic.core.context;
using logic.core.factories;
using logic.core.util;

namespace logic.essential.reward.result
{
    public class CompositeRewardResult : RewardResult
    {
        public IRewardResult[] results { get; private set; }

        public CompositeRewardResult(string type, IRewardResult[] results, IContext context = null)
            : base(type, context)
        {
            this.results = results;
        }

        public CompositeRewardResult(RawNode node, IContext context) : base(node, context)
        {
            var rawResults = node.GetRawNodeArray("results");
            results = new IRewardResult[rawResults.Length];

            for (var i = 0; i < rawResults.Length; i++)
            {
                results[i] = FactoryManager.Build<RewardResult>(rawResults[i], context);
            }
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict().SetArgs("type", type, "results", SerializeUtil.SerializeArray(results));
        }
    }
}