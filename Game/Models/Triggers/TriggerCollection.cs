using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Game.Models.Triggers
{
    public class TriggerCollection : ReferenceCollectionBase<Trigger, TriggerCategories, TriggerDescription>
    {
        public TriggerCollection(RawNode initNode, TriggerCategories categories, IContext context, IDescription dataSource) : base(initNode, categories, context, dataSource)
        {
        }

        protected override Trigger Factory(RawNode initNode, TriggerDescription description)
        {
            switch (description.type)
            {
                case AndTrigger.Type:
                    return new AndTrigger(initNode, categories, description, GetContext());
                
                case OrTrigger.Type:
                    return new QueueTrigger(initNode, categories, description, GetContext());
                
                case QueueTrigger.Type:
                    return new QueueTrigger(initNode, categories, description, GetContext());
                
                default:
                    return new Trigger(initNode, categories, description, GetContext());
            }
        }
    }
}