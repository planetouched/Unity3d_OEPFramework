using common;
using logic.core.context;
using logic.core.reference.description;

namespace test.reference.someModel
{
    public class SomeModelDataSource : DataSourceBase<SomeModelDescription>
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
