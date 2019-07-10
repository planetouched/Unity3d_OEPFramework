using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Factories;
using Basement.BLFramework.Core.Model;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;
using Game.Models.Triggers._Base;

namespace Game.Models.Triggers
{
    public class TriggerCollection : ReferenceCollectionBase<Trigger, TriggerCategories, TriggerDescription>
    {
        public TriggerCollection(RawNode initNode, TriggerCategories categories, IContext context, IDescription dataSource) : base(initNode, categories, context, dataSource)
        {
        }

        protected override Trigger Factory(RawNode initNode, TriggerDescription description)
        {
            return (Trigger)FactoryManager.Build(typeof(Trigger), description.type, initNode, categories, description, GetContext());
        }
    }
}