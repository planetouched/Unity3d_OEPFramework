using common;
using logic.core.context;
using logic.core.factories;
using logic.core.reference.description;
using logic.core.throughEvent;
using logic.essential.choice;
using logic.essential.path;
using logic.essential.reward.result;

namespace logic.essential.reward
{
    public class Reward : DescriptionBase, IReward
    {
        public string type { get; private set; }
        private readonly IPathChoice choice;
        private readonly ModelsPath path;

        public Reward(RawNode node, IContext context = null) : base(node, context)
        {
            type = node.GetString("type");

            if (node.CheckKey("choice"))
            {
                choice = FactoryManager.Build<PathChoice>(node.GetNode("choice"), context);
            }
            else
            {
                if (node.CheckKey("path"))
                {
                    path = PathUtil.GetModelPath(GetContext(), node.GetNode("path"));
                }
            }
        }

        public IRewardResult Award()
        {
            return Award(Calculate());
        }

        public virtual IRewardResult Award(IRewardResult rewardResult)
        {
            return null;
        }

        public virtual IRewardResult Calculate()
        {
            return new RewardResult();
        }

        protected ModelsPath RewardPath()
        {
            return choice != null ? choice.GetModelPath() : path;
        }
    }
}
