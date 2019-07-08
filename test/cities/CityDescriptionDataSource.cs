using common;
using logic.core.context;
using logic.core.reference.description;

namespace Assets.test.cities
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
