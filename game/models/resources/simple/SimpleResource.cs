using System;
using common;
using logic.core.context;
using logic.core.model;
using logic.core.util;

namespace Assets.game.models.resources.simple
{
    public class SimpleResource : ReferenceModelBase<SimpleResourceCategories, SimpleResourceDescription>
    {
        public int amount { get; private set; }

        public SimpleResource(RawNode initNode, SimpleResourceCategories categories, SimpleResourceDescription description, IContext context) : base(initNode, categories, description, context)
        {
            amount = initNode.GetInt("amount");
        }

        public void Change(int addAmount)
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
