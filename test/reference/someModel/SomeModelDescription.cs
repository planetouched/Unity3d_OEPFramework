using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.factories;
using Assets.logic.core.reference.description;
using Assets.logic.essential.amount;
using Assets.logic.essential.price;
using Assets.logic.essential.requirement;
using Assets.logic.essential.reward;

namespace Assets.test.reference.someModel
{
    public class SomeModelDescription : SelectableDescriptionBase
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
