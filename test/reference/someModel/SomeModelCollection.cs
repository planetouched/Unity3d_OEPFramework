using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.model;
using Assets.logic.core.reference.dataSource;

namespace Assets.test.reference.someModel
{
    public class SomeModelCollection : ReferenceCollectionBase<SomeModel, SomeModelCategories, SomeModelDescription>
    {
        public SomeModelCollection(RawNode initNode, SomeModelCategories categories, IContext context, IDataSource<string, SomeModelDescription> dataSource) : base(initNode, categories, context, dataSource)
        {
        }

        protected override SomeModel Factory(RawNode initNode, SomeModelDescription description)
        {
            return new SomeModel(initNode, categories, description, GetContext());
        }
    }
}
