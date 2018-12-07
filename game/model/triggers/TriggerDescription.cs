using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.factories;
using Assets.logic.core.reference.description;
using Assets.logic.essential.reward;

namespace Assets.game.model.triggers
{
    public class TriggerDescription : SelectableDescriptionBase
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
