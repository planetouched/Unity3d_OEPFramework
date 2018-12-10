using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.model;
using Assets.logic.core.reference.dataSource;

namespace Assets.game.model.resource.limited
{
    public class LimitedResourceCollection : ReferenceCollectionBase<LimitedResource, LimitedResourceCategories, LimitedResourceDescription>
    {
        public LimitedResourceCollection(RawNode initNode, LimitedResourceCategories categories, IContext context, IDataSource<string, LimitedResourceDescription> dataSource) : base(initNode, categories, context, dataSource)
        {
        }

        protected override LimitedResource Factory(RawNode initNode, LimitedResourceDescription description)
        {
            return new LimitedResource(initNode, categories, description, GetContext());
        }
    }
}
