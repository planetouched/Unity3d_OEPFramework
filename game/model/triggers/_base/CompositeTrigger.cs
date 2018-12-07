using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.util;

namespace Assets.game.model.triggers._base
{
    public abstract class CompositeTrigger : Trigger
    {
        public TriggerCollection triggers { get; private set; }

        protected CompositeTrigger(RawNode initNode, TriggerDescription description, TriggerCategories categories, IContext context) 
            : base(initNode, description, categories, context)
        {
            triggers = new TriggerCollection(initNode.GetNode("triggers"), categories, context, new TriggerDataSource(description.GetNode().GetNode("triggers"), context));
        }

        public override void Destroy()
        {
            triggers.Destroy();
            base.Destroy();
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict(base.Serialize()).SetArgs("triggers", triggers.Serialize());
        }
    }
}