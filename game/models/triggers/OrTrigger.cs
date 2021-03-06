using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.ThroughEvent;
using Basement.Common;
using Game.Models.Triggers._Base;

namespace Game.Models.Triggers
{
    public class OrTrigger : CompositeTrigger
    {
        public OrTrigger(RawNode initNode, TriggerCategories categories, TriggerDescription description, IContext context)
            : base(initNode, categories, description, context)
        {
        }

        protected override void OnActivated()
        {
            triggers.Attach(triggers.categories.completed, OnTriggerCompleted);
            foreach (var triggerPair in triggers)
            {
                var trigger = triggerPair.Value;
                trigger.Activate();
            }

            CheckCompleted();
        }

        private void OnTriggerCompleted(CoreParams cp, object args)
        {
            CheckCompleted();
        }

        private void CheckCompleted()
        {
            foreach (var triggerPair in triggers)
            {
                var trigger = triggerPair.Value;
                if (trigger.completed)
                {
                    Complete();
                }
            }
        }

        protected override void OnDeactivated()
        {
            triggers.Detach(triggers.categories.completed, OnTriggerCompleted);

            foreach (var triggerPair in triggers)
            {
                var trigger = triggerPair.Value;
                trigger.Deactivate();
            }
        }
    }
}