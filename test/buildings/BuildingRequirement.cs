using common;
using logic.core.context;
using logic.essential.requirement;

namespace Assets.test.buildings
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
