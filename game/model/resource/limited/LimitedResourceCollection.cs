using common;
using logic.core.context;
using logic.core.model;
using logic.core.reference.dataSource;

namespace game.model.resource.limited
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
