using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Factories;
using Basement.BLFramework.Core.Reference.Description;
using Basement.BLFramework.Essential.Prices;
using Basement.BLFramework.Essential.Rewards;
using Basement.Common;

namespace Game.Models.Triggers
{
    public class TriggerDescription : DescriptionBase
    {
        public string type { get; }
        public bool acceptable { get; }
        public IReward reward { get; private set; }
        public IReward acceptedReward { get; private set; }
        public IPrice price { get; private set; }

        public TriggerDescription(RawNode node, IContext context) : base(node, context)
        {
            type = node.GetString("type");
            acceptable = node.GetBool("acceptable");
        }

        public override void Initialization()
        {
            reward = FactoryManager.Build<Reward>(node.GetNode("reward"), GetContext());
            acceptedReward = FactoryManager.Build<Reward>(node.GetNode("accepted-reward"), GetContext());
            price = FactoryManager.Build<Price>(node.GetNode("price"), GetContext());
        }
    }
}
