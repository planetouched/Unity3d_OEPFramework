using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Game.Models.Resources.Simple
{
    public class SimpleResourceDescription : DescriptionBase
    {
        public SimpleResourceDescription(RawNode node, IContext context = null) : base(node, context)
        {
        }
    }
}
