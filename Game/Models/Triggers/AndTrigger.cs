using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.ThroughEvent;
using Basement.Common;
using Game.Models.Triggers._Base;

namespace Game.Models.Triggers
{
    public class AndTrigger : CompositeTrigger
    {
        public const string Type = "and";
        
        public AndTrigger(RawNode initNode, TriggerCategories categories, TriggerDescription description, IContext context)
            : base(initNode, categories, description, context)
        {
        }

        protected override void OnActivate()
        {
            foreach (var triggerPair in triggers)
            {
                var trigger = triggerPair.Value;
                trigger.Activate();
            }

            triggers.Attach(triggers.categories.completed, OnTriggerCompleted);
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
                if (!trigger.completed)
                {
                    return;
                }
            }

            Complete();
        }

        protected override void OnDeactivate()
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