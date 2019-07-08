using common;
using logic.core.context;
using logic.essential.requirement;

namespace Assets.test.cities
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
