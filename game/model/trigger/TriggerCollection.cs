using common;
using game.model.trigger._base;
using logic.core.context;
using logic.core.factories;
using logic.core.model;
using logic.core.reference.dataSource;

namespace game.model.trigger
{
    public class TriggerCollection : ReferenceCollectionBase<Trigger, TriggerCategories, TriggerDescription>
    {
        public TriggerCollection(RawNode initNode, TriggerCategories categories, IContext context, IDataSource<string, TriggerDescription> dataSource) : base(initNode, categories, context, dataSource)
        {
        }

        protected override Trigger Factory(RawNode initNode, TriggerDescription description)
        {
            return (Trigger)FactoryManager.Build(typeof(Trigger), description.type, initNode, categories, description, GetContext());
        }
    }
}