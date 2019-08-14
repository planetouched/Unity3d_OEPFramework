using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;

namespace Test.Simple
{
    public class Territory : ModelBase
    {
        public TerritoryCategory category;
        public Tanks tanks;

        public Territory(string key, TerritoryCategory category, IContext context, IModel parent = null) : base(key, context, parent)
        {
            this.category = category;

            tanks = new Tanks("tanks", category.tank, context, this);
        }
    }
}
