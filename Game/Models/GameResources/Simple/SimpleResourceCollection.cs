using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Game.Models.GameResources.Simple
{
    public class SimpleResourceCollection : ReferenceCollectionBase<SimpleResource, SimpleResourceCategories, SimpleResourceDescription>
    {
        public SimpleResourceCollection(RawNode initNode, SimpleResourceCategories categories, IContext context, IDescription dataSource) : base(initNode, categories, context, dataSource)
        {
        }

        protected override SimpleResource Factory(RawNode initNode, SimpleResourceDescription description)
        {
            return new SimpleResource(initNode, categories, description, GetContext());
        }
    }
}
