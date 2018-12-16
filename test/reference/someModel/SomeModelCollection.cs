using common;
using logic.core.context;
using logic.core.model;
using logic.core.reference.description;

namespace test.reference.someModel
{
    public class SomeModelCollection : ReferenceCollectionBase<SomeModel, SomeModelCategories, SomeModelDescription>
    {
        public SomeModelCollection(RawNode initNode, SomeModelCategories categories, IContext context, IDescription dataSource) : base(initNode, categories, context, dataSource)
        {
        }

        protected override SomeModel Factory(RawNode initNode, SomeModelDescription description)
        {
            return new SomeModel(initNode, categories, description, GetContext());
        }
    }
}
