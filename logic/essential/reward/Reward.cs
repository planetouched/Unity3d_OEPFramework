using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.factories;
using Assets.logic.core.reference.description;
using Assets.logic.essential.choice;
using Assets.logic.essential.path;
using Assets.logic.essential.reward.result;

namespace Assets.logic.essential.reward
{
    public class Reward : DescriptionBase, IReward
    {
        public string type { get; private set; }
        private readonly IPathChoice choice;
        private readonly Path path;

        public Reward(RawNode node, IContext context = null) : base(node, context)
        {
            type = node.GetString("type");

            if (node.CheckKey("choice"))
            {
                choice = FactoryManager.Build<PathChoice>(node.GetNode("choice"), context);
            }
            else
            {
                path = Path.Create(GetContext(), node.GetNode("path"));
            }
        }

        public IRewardResult Award()
        {
            return OnAward();
        }

        protected virtual IRewardResult OnAward()
        {
            return new RewardResult(null);
        }

        protected Path RewardPath()
        {
            return choice != null ? choice.GetPath() : path;
        }
    }
}
