using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Essential.Requirement;
using Basement.Common;

namespace Test.Buildings
{
    public class BuildingRequirement : Requirement
    {
        public BuildingRequirement(RawNode node, IContext context) : base(node, context)
        {
        }

        public override bool Check()
        {
            return GetPath().GetSelf<Building>().completed;
        }
    }
}
