using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Test.Buildings
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
