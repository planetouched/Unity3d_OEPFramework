﻿using Assets.common;
using Assets.logic.core.context;

namespace Assets.logic.essential.requirement
{
    public class NotRequirement : WrappedRequirement
    {
        public NotRequirement(RawNode node, IContext context) : base(node, context)
        {
        }

        public override bool Check()
        {
            return !innerRequirement.Check();
        }
    }
}
