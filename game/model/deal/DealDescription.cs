using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.factories;
using Assets.logic.core.reference.description;
using Assets.logic.essential.price;
using Assets.logic.essential.requirement;
using Assets.logic.essential.reward;
using Assets.logic.essential.reward.result;

namespace Assets.game.model.deal
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