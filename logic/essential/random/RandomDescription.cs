﻿using common;
using logic.core.context;
using logic.core.reference.description;

namespace logic.essential.random
{
    public class RandomDescription : SelectableDescriptionBase
    {
        public string type { get; private set; }

        public RandomDescription(RawNode node, IContext context = null) : base(node, context)
        {
            type = node.GetString("type");
        }
    }
}
