using common;
using logic.core.context;
using logic.core.reference.description;

namespace logic.essential.random
{
    public class RandomDataSource : DataSourceBase<RandomDescription>
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
