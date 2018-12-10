using Assets.logic.core.context;
using Assets.logic.core.model;

namespace Assets.test.simple
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
