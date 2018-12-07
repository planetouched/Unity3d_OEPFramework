using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.model;
using Assets.logic.core.reference.dataSource;

namespace Assets.game.model.resource.renewableResource
{
    public class RenewableResourceCollection : ReferenceCollectionBase<RenewableResource, RenewableResourceDescription>
    {
        private readonly RenewableResourceCategories categories;

        public RenewableResourceCollection(RawNode initNode, RenewableResourceCategories categories, IContext context, IDataSource<string, RenewableResourceDescription> dataSource) : base(initNode, context, dataSource)
        {
            this.categories = categories;
        }

        protected override RenewableResource Factory(RawNode initNode, RenewableResourceDescription description, IContext context)
        {
            return new RenewableResource(initNode, description, categories, context);
        }
    }
}
