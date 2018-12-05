using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.dataSource;

namespace Assets.logic.essential.random
{
    public class RandomDataSource : DataSourceDescriptionBase<RandomDescription>
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
