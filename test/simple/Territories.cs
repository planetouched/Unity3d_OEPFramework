using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;

namespace Test.Simple
{
    public class Territories : CollectionBase<Territory>
    {
        public TerritoryCategory category;

        public Territories(string key, TerritoryCategory category, IContext context) : base(key, context, null)
        {
            this.category = category;
            AddChild(new Territory("territory0", category, context));
        }
    }
}
