using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Util;
using Basement.Common;

namespace Game.Models.Triggers._Base
{
    public abstract class CounterTrigger : Trigger
    {
        public int count { get; protected set; }
        public int maxCount { get; private set; }

        protected CounterTrigger(RawNode initNode, TriggerCategories categories, TriggerDescription description, IContext context) 
            : base(initNode, categories, description, context)
        {
            count = initNode.GetInt("count");
            maxCount = description.GetNode().GetInt("max-count");
        }

        protected virtual void IncrementCount(int delta)
        {
            count += delta;

            if (count >= maxCount)
            {
                Complete();
            }
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict(base.Serialize()).SetArgs("count", count);
        }
    }
}