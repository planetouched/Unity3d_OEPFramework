using common;
using logic.core.context;
using logic.core.model;
using logic.core.reference.description;

namespace Assets.game.models.resources.simple
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
