using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.util;

namespace Assets.game.model.trigger._base
{
    public abstract class CounterTrigger : Trigger
    {
        public int count { get; protected set; }
        public int maxCount { get; private set; }
        public int additionalCount { get; private set; }

        protected CounterTrigger(RawNode initNode, TriggerDescription description, TriggerCategories categories, IContext context) 
            : base(initNode, description, categories, context)
        {
            count = initNode.GetInt("count");
            additionalCount = initNode.GetInt("additional-count");
            maxCount = description.GetNode().GetInt("max-count");
        }

        protected virtual void IncrementCount(int delta)
        {
            count += delta;

            if (count >= maxCount + additionalCount)
            {
                Complete();
            }
        }

        public void SetAdditionCount(int amount)
        {
            int oldCount = additionalCount;
            additionalCount = amount;
            Call(categories.update, new TriggerHandlerArgs { triggerData = new CounterTriggerData { newAdditionCount = additionalCount, oldAdditionCount = oldCount } });
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict(base.Serialize()).SetArgs("count", count, "additional-count", additionalCount);
        }
    }
}