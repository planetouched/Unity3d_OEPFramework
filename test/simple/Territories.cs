using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;

namespace Test.Simple
{
    public class Territories : CollectionBase<Territory>
    {
        public TerritoryCategory category;

        public Territories(TerritoryCategory category, IContext context) : base(context, null)
        {
            this.category = category;
            AddChild("0", new Territory(category, context));
        }
    }
}
