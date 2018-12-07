using Assets.common;
using Assets.game.model.triggers._base;
using Assets.logic.core.context;
using Assets.logic.core.throughEvent;

namespace Assets.game.model.triggers
{
    public class OrTrigger : CompositeTrigger
    {
        public OrTrigger(RawNode initNode, TriggerDescription description, TriggerCategories categories, IContext context)
            : base(initNode, description, categories, context)
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