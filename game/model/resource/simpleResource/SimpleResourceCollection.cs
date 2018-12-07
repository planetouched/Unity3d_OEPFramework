using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.model;
using Assets.logic.core.reference.dataSource;

namespace Assets.game.model.resource.simpleResource
{
    public class SimpleResourceCollection : ReferenceCollectionBase<SimpleResource, SimpleResourceDescription>
    {
        public readonly SimpleResourceCategories categories;

        public SimpleResourceCollection(RawNode initNode, SimpleResourceCategories categories, IContext context, IDataSource<string, SimpleResourceDescription> dataSource) : base(initNode, context, dataSource)
        {
            this.categories = categories;
        }

        protected override SimpleResource Factory(RawNode initNode, SimpleResourceDescription description, IContext context)
        {
            return new SimpleResource(initNode, categories, description, context);
        }
    }
}
