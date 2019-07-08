using common;
using logic.core.context;
using logic.core.model;
using logic.core.reference.description;

namespace Assets.test.buildings
{
    public class BuildingCollection : ReferenceCollectionBase<Building, BuildingCategories, BuildingDescription>
    {
        public BuildingCollection(RawNode initNode, BuildingCategories categories, IContext context, IDescription dataSource) : base(initNode, categories, context, dataSource)
        {
        }

        protected override Building Factory(RawNode initNode, BuildingDescription description)
        {
            return new Building(initNode, categories, description, GetContext());
        }
    }
}
