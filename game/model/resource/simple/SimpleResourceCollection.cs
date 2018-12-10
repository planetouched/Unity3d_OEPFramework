using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.model;
using Assets.logic.core.reference.dataSource;

namespace Assets.game.model.resource.simple
{
    public class SimpleResourceCollection : ReferenceCollectionBase<SimpleResource, SimpleResourceCategories, SimpleResourceDescription>
    {
        public SimpleResourceCollection(RawNode initNode, SimpleResourceCategories categories, IContext context, IDataSource<string, SimpleResourceDescription> dataSource) : base(initNode, categories, context, dataSource)
        {
        }

        protected override SimpleResource Factory(RawNode initNode, SimpleResourceDescription description)
        {
            return new SimpleResource(initNode, categories, description, GetContext());
        }
    }
}
