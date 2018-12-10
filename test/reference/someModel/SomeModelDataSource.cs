using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.dataSource;

namespace Assets.test.reference.someModel
{
    public class SomeModelDataSource : DataSourceDescriptionBase<SomeModelDescription>
    {
        public SomeModelDataSource(RawNode node, IContext context = null) : base(node, context)
        {
        }

        protected override SomeModelDescription Factory(RawNode node)
        {
            return new SomeModelDescription(node, GetContext());
        }
    }
}
