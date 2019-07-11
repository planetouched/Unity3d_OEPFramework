using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Essential.Requirements;
using Basement.Common;

namespace Test.Cities
{
    public class CityRequirement : Requirement
    {
        public CityRequirement(RawNode node, IContext context) : base(node, context)
        {
        }

        public override bool Check()
        {
            var city = GetPath().GetSelf<City>();
            return city.completed;
        }
    }
}
