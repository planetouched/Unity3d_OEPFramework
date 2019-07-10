using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Game.Models.Resources.Limited
{
    public class LimitedResourceCollection : ReferenceCollectionBase<LimitedResource, LimitedResourceCategories, LimitedResourceDescription>
    {
        public LimitedResourceCollection(RawNode initNode, LimitedResourceCategories categories, IContext context, IDescription dataSource) : base(initNode, categories, context, dataSource)
        {
        }

        protected override LimitedResource Factory(RawNode initNode, LimitedResourceDescription description)
        {
            return new LimitedResource(initNode, categories, description, GetContext());
        }
    }
}
