using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;

namespace Test.Simple
{
    public class Tank : ModelBase
    {
        private TankCategory category;

        public Tank(TankCategory category, IContext context, IModel parent = null) : base(context, parent)
        {
            this.category = category;
        }

        public void Fire()
        {
            Call(category.fire, new TankHandlerArgs {damage = 600});
        }
    }
}
