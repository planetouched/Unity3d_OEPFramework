using common;
using logic.core.context;
using logic.core.reference.description;

namespace Assets.test.buildings
{
    public class BuildingDataSource : DataSourceBase<BuildingDescription>
    {
        public BuildingDataSource(RawNode node, IContext context = null) : base(node, context)
        {
        }

        protected override BuildingDescription Factory(RawNode partialNode)
        {
            return new BuildingDescription(partialNode, GetContext());
        }
    }
}
