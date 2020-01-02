using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Game.Models.GameResources.Renewable
{
    public class RenewableResourceDescription : DescriptionBase
    {
        public int recoveryTime { get; }
        public int renewableMaximum { get; }
        public int recoveryStep { get; }
        public int maximum { get; }

        public RenewableResourceDescription(RawNode node, IContext context = null) : base(node, context)
        {
            recoveryTime = node.GetInt("recovery-time");
            renewableMaximum = node.GetInt("renewable-maximum");
            recoveryStep = node.GetInt("recovery-step");
            maximum = node.GetInt("maximum");
        }
    }
}
