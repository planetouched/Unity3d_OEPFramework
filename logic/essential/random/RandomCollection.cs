using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.model;
using Assets.logic.core.reference.dataSource;

namespace Assets.logic.essential.random
{
    public class RandomCollection : ReferenceCollectionBase<Random, RandomCategories, RandomDescription>
    {
        public RandomCollection(RawNode initNode, RandomCategories categories, IContext context, IDataSource<string, RandomDescription> dataSource) : base(initNode, categories, context, dataSource)
        {
        }

        protected override Random Factory(RawNode initNode, RandomDescription description)
        {
            return new Random(initNode, categories, description, GetContext());
        }
    }
}
