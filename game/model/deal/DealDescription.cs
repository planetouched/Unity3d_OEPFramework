using common;
using logic.core.context;
using logic.core.factories;
using logic.core.reference.description;
using logic.essential.price;
using logic.essential.requirement;
using logic.essential.reward;
using logic.essential.reward.result;

namespace game.model.deal
{
    public class DealDescription : SelectableDescriptionBase
    {
        public IRequirement requirement { get; private set; }
        public IPrice price { get; private set; }
        public IReward reward { get; private set; }

        public DealDescription(RawNode node, IContext context) : base(node, context)
        {
        }

        public override void Initialization()
        {
            requirement = FactoryManager.Build<Requirement>(node.GetNode("requirement"), GetContext());
            price = FactoryManager.Build<Price>(node.GetNode("price"), GetContext());
            reward = FactoryManager.Build<Reward>(node.GetNode("reward"), GetContext());
        }

        public bool Check()
        {
            return requirement.Check() && price.Check();
        }

        public IRewardResult Make()
        {
            if (!Check())
            {
                return null;
            }

            price.Pay();
            return reward.Award();
        }
    }
}