using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.model;
using Assets.logic.core.reference.dataSource;

namespace Assets.game.model.resource.renewable
{
    public class RenewableResourceCollection : ReferenceCollectionBase<RenewableResource, RenewableResourceCategories, RenewableResourceDescription>
    {
        public RenewableResourceCollection(RawNode initNode, RenewableResourceCategories categories, IContext context, IDataSource<string, RenewableResourceDescription> dataSource) : base(initNode, categories, context, dataSource)
        {
        }

        protected override RenewableResource Factory(RawNode initNode, RenewableResourceDescription description)
        {
            return new RenewableResource(initNode, categories, description, GetContext());
        }
    }
}
