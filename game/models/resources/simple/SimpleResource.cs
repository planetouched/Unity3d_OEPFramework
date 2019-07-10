using System;
using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;
using Basement.BLFramework.Core.Util;
using Basement.Common;

namespace Game.Models.Resources.Simple
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
