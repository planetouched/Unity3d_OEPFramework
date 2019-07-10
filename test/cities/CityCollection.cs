using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Test.Cities
{
    public class CityCollection : ReferenceCollectionBase<City, CityCategories, CityDescription>
    {
        public CityCollection(RawNode initNode, CityCategories categories, IContext context, IDescription dataSource) : base(initNode, categories, context, dataSource)
        {
        }

        protected override City Factory(RawNode initNode, CityDescription description)
        {
            return new City(initNode, categories, description, GetContext());
        }
    }
}
