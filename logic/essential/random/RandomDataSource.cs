using common;
using logic.core.context;
using logic.core.reference.dataSource;

namespace logic.essential.random
{
    public class RandomDataSource : SelectableDataSourceBase<RandomDescription>
    {
        public RandomDataSource(RawNode node, IContext context = null) : base(node, context)
        {
        }

        protected override RandomDescription Factory(RawNode node)
        {
            return new RandomDescription(node);
        }
    }
}
