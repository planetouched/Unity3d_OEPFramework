using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.model;
using Assets.logic.core.reference.dataSource;

namespace Assets.logic.essential.random
{
    public class RandomCollection : ReferenceCollectionBase<Random, RandomDescription>
    {
        public RandomCollection(RawNode initNode, IContext context, IDataSource<string, RandomDescription> dataSource) : base(initNode, context, dataSource)
        {
        }

        protected override Random Factory(RawNode initNode, RandomDescription description, IContext context)
        {
            return new Random(initNode, description, context);
        }
    }
}
