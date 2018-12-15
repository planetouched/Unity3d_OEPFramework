using common;
using logic.core.context;
using logic.core.model;
using logic.core.reference.dataSource;

namespace logic.essential.random
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
