using Assets.common;
using Assets.game.model.trigger._base;
using Assets.logic.core.context;
using Assets.logic.core.factories;
using Assets.logic.core.model;
using Assets.logic.core.reference.dataSource;

namespace Assets.game.model.trigger
{
    public class TriggerCollection : ReferenceCollectionBase<Trigger, TriggerDescription>
    {
        public TriggerCategories categories { get; private set; }

        public TriggerCollection(RawNode initNode, TriggerCategories categories, IContext context, IDataSource<string, TriggerDescription> dataSource) : base(initNode, context, dataSource)
        {
            this.categories = categories;
        }

        protected override Trigger Factory(RawNode initNode, TriggerDescription description, IContext context)
        {
            return (Trigger)FactoryManager.Build(typeof(Trigger), description.type, initNode, description, categories, context);
        }
    }
}