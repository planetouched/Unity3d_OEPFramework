using common;
using logic.core.context;
using logic.core.model;
using logic.core.reference.description;

namespace Assets.test.cities
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
