using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.ThroughEvent;
using Basement.BLFramework.Core.Util;
using Basement.Common;
using Game.Models.Triggers._Base;

namespace Game.Models.Triggers
{
    public class QueueTrigger : CompositeTrigger
    {
        public const string Type = "queue";
        
        private int _step;

        public Trigger currentStep => GetStep(_step);

        private Trigger nextStep => GetStep(_step + 1);

        public QueueTrigger(RawNode initNode, TriggerCategories categories, TriggerDescription description, IContext context) 
            : base(initNode, categories, description, context)
        {
            _step = initNode.GetInt("step");
        }

        private void ActivateCurrentStep()
        {
            currentStep.Attach(currentStep.categories.completed, OnCurrentTriggerCompleted);
            currentStep.Activate();
        }

        protected override void OnActivate()
        {
            ActivateCurrentStep();
        }

        private void OnCurrentTriggerCompleted(CoreParams cp, object args)
        {
            currentStep.Detach(currentStep.categories.completed, OnCurrentTriggerCompleted);

            if (nextStep == null)
            {
                Complete();
            }
            else
            {
                _step++;
                ActivateCurrentStep();
            }
        }

        private Trigger GetStep(int i)
        {
            string stepId = i.ToString();

            if (triggers.Exist(stepId))
            {
                return triggers[stepId];
            }

            return null;
        }

        protected override void OnDeactivate()
        {
            currentStep.Detach(currentStep.categories.completed, OnCurrentTriggerCompleted);
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict(base.Serialize()).SetArgs("step", _step);
        }
    }
}