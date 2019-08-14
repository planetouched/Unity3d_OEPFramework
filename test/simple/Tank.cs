using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;

namespace Test.Simple
{
    public class Tank : ModelBase
    {
        private TankCategory category;

        public Tank(string key, TankCategory category, IContext context, IModel parent = null) : base(key, context, parent)
        {
            this.category = category;
        }

        public void Fire()
        {
            Call(category.fire, new TankHandlerArgs {damage = 600});
        }
    }
}
