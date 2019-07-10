using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Test.Reference.someModel
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
