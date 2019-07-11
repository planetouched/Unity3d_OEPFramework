using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Factories;
using Basement.BLFramework.Core.Reference.Description;
using Basement.BLFramework.Essential.Requirements;
using Basement.Common;

namespace Test.Cities
{
    public class CityDescription : DescriptionBase
    {
        public IRequirement requirement { get; private set; }

        public CityDescription(RawNode node, IContext context = null) : base(node, context)
        {
        }

        public override void Initialization()
        {
            requirement = FactoryManager.Build<Requirement>(node.GetNode("requirement"), GetContext());
        }
    }
}
