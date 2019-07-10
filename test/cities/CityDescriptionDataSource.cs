using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Test.Cities
{
    public class CityDescriptionDataSource : DataSourceBase<CityDescription>
    {
        public CityDescriptionDataSource(RawNode node, IContext context = null) : base(node, context)
        {
        }

        protected override CityDescription Factory(RawNode partialNode)
        {
            return new CityDescription(partialNode, GetContext());
        }
    }
}
