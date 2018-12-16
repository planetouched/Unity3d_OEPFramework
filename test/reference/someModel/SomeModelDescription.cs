using common;
using logic.core.context;
using logic.core.factories;
using logic.core.reference.description;
using logic.essential.amount;
using logic.essential.price;
using logic.essential.requirement;
using logic.essential.reward;

namespace test.reference.someModel
{
    public class SomeModelDescription : DescriptionBase
    {
        public IPrice price { get; private set; }
        public IReward reward { get; private set; }
        public IRequirement requirement { get; private set; }
        public IAmount amount { get; private set; }

        public SomeModelDescription(RawNode node, IContext context = null) : base(node, context)
        {
        }

        public override void Initialization()
        {
            price = FactoryManager.Build<Price>(node.GetNode("price"), GetContext());
            reward = FactoryManager.Build<Reward>(node.GetNode("reward"), GetContext());
            requirement = FactoryManager.Build<Requirement>(node.GetNode("requirement"), GetContext());
            amount = FactoryManager.Build<Amount>(node.GetNode("amount"), GetContext());
        }
    }
}
