using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Util;
using Basement.Common;

namespace Game.Models.Triggers._Base
{
    public abstract class CompositeTrigger : Trigger
    {
        private const string TriggersKey = "triggers";
        
        public TriggerCollection triggers { get; }

        protected CompositeTrigger(RawNode initNode, TriggerCategories categories, TriggerDescription description, IContext context) 
            : base(initNode, categories, description, context)
        {
            triggers = new TriggerCollection(initNode.GetNode(TriggersKey), categories, context, new TriggerDataSource(description.GetNode().GetNode(TriggersKey), context));
        }

        public override void Destroy()
        {
            triggers.Destroy();
            base.Destroy();
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict(base.Serialize()).SetArgs(TriggersKey, triggers.Serialize());
        }
    }
}