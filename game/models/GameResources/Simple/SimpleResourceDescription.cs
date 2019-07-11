using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Game.Models.GameResources.Simple
{
    public class SimpleResourceDescription : DescriptionBase
    {
        public SimpleResourceDescription(RawNode node, IContext context = null) : base(node, context)
        {
        }
    }
}
