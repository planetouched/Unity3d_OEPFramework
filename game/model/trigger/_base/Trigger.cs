using common;
using logic.core.context;
using logic.core.model;
using logic.core.util;

namespace game.model.trigger._base
{
    public class Trigger : ReferenceModelBase<TriggerCategories, TriggerDescription>
    {
        public bool completed { get; private set; }
        public bool activated { get; private set; }

        public Trigger(RawNode initNode, TriggerCategories categories, TriggerDescription description, IContext context): base(initNode, categories, description, context)
        {
            this.categories = categories;
            activated = initNode.GetBool("activated");
            completed = initNode.GetBool("completed");
        }

        public override void Initialization()
        {
            if (activated)
                InnerActivate();
        }

        public void Activate()
        {
            if (activated) return;

            activated = true;
            completed = false;

            InnerActivate();
            Call(categories.activated, new TriggerHandlerArgs());
        }

        public void Deactivate()
        {
            if (!activated) return;
            activated = false;
            OnDeactivated();
        }

        private void InnerActivate()
        {
            OnActivated();
        }

        protected virtual void OnActivated()
        {
        }

        protected virtual void OnDeactivated()
        {
        }

        protected void Complete()
        {
            if (completed) return;

            Deactivate();
            completed = true;
            description.reward.Award();
            Call(categories.completed, new TriggerHandlerArgs());
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict().SetArgs("completed", completed, "activated", activated);
        }
    }
}