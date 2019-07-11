using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Factories;
using Basement.BLFramework.Core.Model;
using Basement.BLFramework.Essential.Rewards.Result;
using Basement.Common;

namespace Test.Reference.someModel
{
    public class SomeModel : ReferenceModelBase<SomeModelCategories, SomeModelDescription>
    {
        public SomeModel(RawNode initNode, SomeModelCategories categories, SomeModelDescription description, IContext context) : base(initNode, categories, description, context)
        {
            int value = description.amount.Number();

            var result = description.reward.Calculate();
            var s = result.Serialize();
            var sResult = FactoryManager.Build<RewardResult>(new RawNode(s), context);

            description.reward.Award(sResult);

            description.reward.Award();
            description.reward.Award();
            description.reward.Award();
            description.reward.Award();

            if (description.requirement.Check())
            {
                description.price.Pay();
            }
        }
    }
}
