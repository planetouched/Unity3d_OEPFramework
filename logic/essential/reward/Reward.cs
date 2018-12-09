using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.factories;
using Assets.logic.core.reference.description;
using Assets.logic.core.throughEvent;
using Assets.logic.essential.choice;
using Assets.logic.essential.path;
using Assets.logic.essential.reward.result;

namespace Assets.logic.essential.reward
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
                    path = PathUtil.ModelsPath(GetContext(), node.GetNode("path"));
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
            return choice != null ? choice.GetPath() : path;
        }
    }
}
