using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Factories;
using Basement.BLFramework.Core.Reference.Description;
using Basement.BLFramework.Essential.Rewards;
using Basement.Common;

namespace Game.Models.Triggers
{
    public class TriggerDescription : DescriptionBase
    {
        public string type { get; private set; }
        public IReward reward { get; private set; }

        public TriggerDescription(RawNode node, IContext context) : base(node, context)
        {
            type = node.GetString("type");
        }

        public override void Initialization()
        {
            reward = FactoryManager.Build<Reward>(node.GetNode("reward"), GetContext());
        }
    }
}
