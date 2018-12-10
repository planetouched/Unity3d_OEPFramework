using Assets.logic.core.context;
using Assets.logic.core.model;

namespace Assets.test.simple
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
