using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.description;

namespace Assets.game.model.resource.renewableResource
{
    public class RenewableResourceDescription : SelectableDescriptionBase
    {
        public int recoveryTime { get; private set; }
        public int renewableMaximum { get; private set; }
        public int recoveryStep { get; private set; }
        public int maximum { get; private set; }

        public RenewableResourceDescription(RawNode node, IContext context = null) : base(node, context)
        {
            recoveryTime = node.GetInt("recovery_time");
            renewableMaximum = node.GetInt("renewable_maximum");
            recoveryStep = node.GetInt("recovery_step");
            maximum = node.GetInt("maximum");
        }
    }
}
