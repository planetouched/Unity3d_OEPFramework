﻿using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Essential.Price;
using Basement.Common;

namespace Game.Models.Resources.Simple
{
    public class SimpleResourcePrice : Price
    {
        public SimpleResource resource { get; private set; }

        public SimpleResourcePrice(RawNode node, IContext context) : base(node, context)
        {
            resource = GetPath().GetSelf<SimpleResource>();
        }

        public override bool Check()
        {
            return resource.amount >= amount;
        }

        public override void Pay()
        {
            if (!Check()) return;
            resource.Change(-amount);
        }
    }
}
