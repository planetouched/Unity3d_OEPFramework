﻿using common;
using logic.core.context;
using logic.core.model;
using logic.core.reference.dataSource;

namespace game.model.resource.renewable
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
