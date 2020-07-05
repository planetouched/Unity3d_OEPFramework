using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Game.Models.GameResources.Renewable
{
    public class RenewableResourceDescription : DescriptionBase
    {
        public int RecoveryTime { get; }
        public int RenewableMaximum { get; }
        public int RecoveryStep { get; }
        public int Maximum { get; }

        public RenewableResourceDescription(RawNode node, IContext context = null) : base(node, context)
        {
            RecoveryTime = node.GetInt("recovery-time");
            RenewableMaximum = node.GetInt("renewable-maximum");
            RecoveryStep = node.GetInt("recovery-step");
            Maximum = node.GetInt("maximum");
        }
    }
}
