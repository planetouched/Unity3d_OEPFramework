﻿using common;
using logic.core.context;
using logic.core.reference.description;

namespace game.models.deal
{
    public class DealDataSource : DataSourceBase<DealDescription>
    {
        public DealDataSource(RawNode node, IContext context = null) : base(node, context)
        {
        }

        protected override DealDescription Factory(RawNode partialNode)
        {
            var description = new DealDescription(partialNode, GetContext());
            description.Initialization();
            return description;
        }
    }
}
