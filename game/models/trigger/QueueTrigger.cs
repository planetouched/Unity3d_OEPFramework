﻿using Assets.game.models.trigger._base;
using common;
using logic.core.context;
using logic.core.throughEvent;
using logic.core.util;

namespace Assets.game.models.trigger
{
    public class QueueTrigger : CompositeTrigger
    {
        private int step;

        public Trigger currentStep
        {
            get { return GetStep(step); }
        }

        private Trigger nextStep
        {
            get { return GetStep(step + 1); }
        }

        public QueueTrigger(RawNode initNode, TriggerCategories categories, TriggerDescription description, IContext context) 
            : base(initNode, categories, description, context)
        {
            step = initNode.GetInt("step");
        }

        private void ActivateCurrentStep()
        {
            currentStep.Attach(currentStep.categories.completed, OnCurrentTriggerCompleted);
            currentStep.Activate();
        }

        protected override void OnActivated()
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
                step++;
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

        protected override void OnDeactivated()
        {
            currentStep.Detach(currentStep.categories.completed, OnCurrentTriggerCompleted);
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict(base.Serialize()).SetArgs("step", step);
        }
    }
}