using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Factories;
using Basement.BLFramework.Core.Reference.Description;
using Basement.BLFramework.Essential.Price;
using Basement.BLFramework.Essential.Requirement;
using Basement.BLFramework.Essential.Reward;
using Basement.BLFramework.Essential.Reward.Result;
using Basement.Common;

namespace Game.Models.Deals
{
    public class DealDescription : DescriptionBase
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