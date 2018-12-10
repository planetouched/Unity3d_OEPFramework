using Assets.logic.core.context;
using Assets.logic.core.model;
using Assets.logic.core.throughEvent;

namespace Assets.test.simple
{
    public class Tanks : CollectionBase<Tank>
    {
        private TankCategory category;

        public Tanks(TankCategory category, IContext context, IModel parent) : base(context, parent)
        {
            this.category = category;

            for (int i = 0; i < 10; i++)
            {
                var tank = new Tank(category, context);
                AddChild(i.ToString(), tank);
                tank.Attach(category.fire, Func);
            }
        }

        private void Func(CoreParams cp, object args)
        {
            cp.stack.GetSelf<Tank>().Detach(category.fire, Func);
        }
    }
}
