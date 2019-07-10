using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Test.Buildings
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
