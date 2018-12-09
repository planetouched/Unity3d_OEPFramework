﻿using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.util;
using Assets.logic.essential.path;
using Assets.logic.essential.reward.result;

namespace Assets.game.model.resource.renewableResource
{
    public class RenewableResourceRewardResult : RewardResult
    {
        public RenewableResource resource { get; private set; }
        public int amount { get; private set; }

        public RenewableResourceRewardResult(string type, RenewableResource resource, int amount, IContext context)
            : base(type, context)
        {
            this.resource = resource;
            this.amount = amount;
        }

        public RenewableResourceRewardResult(RawNode node, IContext context) : base(node, context)
        {
            resource = PathUtil.ModelsPath(context, node.GetString("path"), null).GetSelf<RenewableResource>();
            amount = node.GetInt("amount");
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict().SetArgs("type", type, "path", PathUtil.StringPath(resource), "amount", amount);
        }
    }
}