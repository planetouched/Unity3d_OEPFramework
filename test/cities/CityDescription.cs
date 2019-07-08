using common;
using logic.core.context;
using logic.core.factories;
using logic.core.reference.description;
using logic.essential.requirement;

namespace Assets.test.cities
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
