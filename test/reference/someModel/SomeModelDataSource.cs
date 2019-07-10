using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Test.Reference.someModel
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
