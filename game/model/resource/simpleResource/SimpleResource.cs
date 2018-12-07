using System;
using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.model;
using Assets.logic.core.util;

namespace Assets.game.model.resource.simpleResource
{
    public class SimpleResource : ReferenceModelBase<SimpleResourceDescription>
    {
        public int amount { get; private set; }
        public SimpleResourceCategories categories { get; private set; }

        public SimpleResource(RawNode initNode, SimpleResourceCategories categories, SimpleResourceDescription description, IContext context) : base(initNode, description, context)
        {
            this.categories = categories;
            amount = initNode.GetInt("amount");
        }

        public void Increment(int addAmount)
        {
            Set(amount + addAmount);
        }

        public void Set(int setAmount)
        {
            if (setAmount < 0)
                throw new Exception("setAmount < 0");

            int old = amount;
            amount = setAmount;
            Call(categories.changed, new SimpleResourceHandlerArgs { oldAmount = old, newAmount = amount });
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict().SetArgs("amount", amount);
        }
    }
}
