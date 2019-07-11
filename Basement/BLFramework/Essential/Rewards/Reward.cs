using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Factories;
using Basement.BLFramework.Core.Reference.Description;
using Basement.BLFramework.Core.ThroughEvent;
using Basement.BLFramework.Essential.Choice;
using Basement.BLFramework.Essential.Path;
using Basement.BLFramework.Essential.Reward.Result;
using Basement.Common;

namespace Basement.BLFramework.Essential.Reward
{
    public class Reward : DescriptionBase, IReward
    {
        public string type { get; }
        private readonly IPathChoice _choice;
        private readonly ModelsPath _path;

        public Reward(RawNode node, IContext context) : base(node, context)
        {
            type = node.GetString("type");

            if (node.CheckKey("choice"))
            {
                _choice = FactoryManager.Build<PathChoice>(node.GetNode("choice"), context);
            }
            else
            {
                if (node.CheckKey("path"))
                {
                    _path = PathUtil.GetModelPath(GetContext(), node.GetNode("path"));
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
            return _choice != null ? _choice.GetModelPath() : _path;
        }
    }
}
