using common;
using logic.core.context;
using logic.core.model;
using logic.core.reference.description;

namespace Assets.game.models.resources.renewable
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
