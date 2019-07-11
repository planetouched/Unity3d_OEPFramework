using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Factories;
using Basement.BLFramework.Core.Reference.Description;
using Basement.BLFramework.Essential.Amounts;
using Basement.BLFramework.Essential.Prices;
using Basement.BLFramework.Essential.Requirements;
using Basement.BLFramework.Essential.Rewards;
using Basement.Common;

namespace Test.Reference.someModel
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
