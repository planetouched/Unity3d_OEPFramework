using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Game.Models.Resources.Renewable
{
    public class RenewableResourceCollection : ReferenceCollectionBase<RenewableResource, RenewableResourceCategories, RenewableResourceDescription>
    {
        public RenewableResourceCollection(RawNode initNode, RenewableResourceCategories categories, IContext context, IDescription dataSource) : base(initNode, categories, context, dataSource)
        {
        }

        protected override RenewableResource Factory(RawNode initNode, RenewableResourceDescription description)
        {
            return new RenewableResource(initNode, categories, description, GetContext());
        }
    }
}
