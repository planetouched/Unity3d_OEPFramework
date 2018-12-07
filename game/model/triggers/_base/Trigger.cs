using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.model;
using Assets.logic.core.util;

namespace Assets.game.model.triggers._base
{
    public class Trigger : ReferenceModelBase<TriggerDescription>
    {
        public TriggerCategories categories { get; private set; }

        public bool completed { get; private set; }
        public bool activated { get; private set; }

        public Trigger(RawNode initNode, TriggerDescription description, TriggerCategories categories, IContext context): base(initNode, description, context)
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
            GetDescription().reward.Award();
            Call(categories.completed, new TriggerHandlerArgs());
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict().SetArgs("completed", completed, "activated", activated);
        }
    }
}