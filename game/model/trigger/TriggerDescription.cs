﻿using common;
using logic.core.context;
using logic.core.factories;
using logic.core.reference.description;
using logic.essential.reward;

namespace game.model.trigger
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
