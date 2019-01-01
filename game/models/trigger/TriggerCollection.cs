using Assets.game.models.trigger._base;
using common;
using logic.core.context;
using logic.core.factories;
using logic.core.model;
using logic.core.reference.description;

namespace Assets.game.models.trigger
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