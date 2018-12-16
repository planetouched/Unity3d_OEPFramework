using logic.core.context;
using logic.core.model;

namespace test.simple
{
    public class Territory : ModelBase
    {
        public TerritoryCategory category;
        public Tanks tanks;

        public Territory(TerritoryCategory category, IContext context, IModel parent = null) : base(context, parent)
        {
            this.category = category;

            tanks = new Tanks(category.tank, context, this);
        }
    }
}
