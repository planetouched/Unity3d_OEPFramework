﻿using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Util;
using Basement.Common;

namespace Game.Models.Triggers._Base
{
    public abstract class CompositeTrigger : Trigger
    {
        public TriggerCollection triggers { get; private set; }

        protected CompositeTrigger(RawNode initNode, TriggerCategories categories, TriggerDescription description, IContext context) 
            : base(initNode, categories, description, context)
        {
            triggers = new TriggerCollection(initNode.GetNode("triggers"), categories, context, new TriggerDataSource(description.GetNode().GetNode("triggers"), context));
        }

        public override void Destroy()
        {
            triggers.Destroy();
            base.Destroy();
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict(base.Serialize()).SetArgs("triggers", triggers.Serialize());
        }
    }
}