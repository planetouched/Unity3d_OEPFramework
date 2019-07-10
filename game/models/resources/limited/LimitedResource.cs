using System;
using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;
using Basement.BLFramework.Core.Util;
using Basement.Common;

namespace Game.Models.Resources.Limited
{
    public class LimitedResource : ReferenceModelBase<LimitedResourceCategories, LimitedResourceDescription>
    {
        public int amount { get; private set; }
        public int additionalMaximum { get; private set; }

        public LimitedResource(RawNode initNode, LimitedResourceCategories categories, LimitedResourceDescription description, IContext context) : base(initNode, categories, description, context)
        {
            amount = initNode.GetInt("amount");
            additionalMaximum = initNode.GetInt("additional-maximum");
        }

        public void Change(int addAmount)
        {
            Set(amount + addAmount);
        }

        public void ChangeAdditinalMaximum(int addMaximum)
        {
            SetAddtionalMaximum(additionalMaximum + addMaximum);
        }

        public void SetAddtionalMaximum(int setAddtitionalMaximum)
        {
            if (setAddtitionalMaximum < 0)
            {
                throw new Exception("setAddtitionalMaximum < 0");
            }

            int old = additionalMaximum;
            additionalMaximum = setAddtitionalMaximum;
            Call(categories.additionalMaximumChanged, new LimitedResourceHandlerArgs { oldAdditionalMaximum = old, newAdditionalMaximum = additionalMaximum });
        }

        public void Set(int setAmount)
        {
            if (setAmount < 0)
            {
                throw new Exception("setAmount < 0");
            }

            int old = amount;
            amount = setAmount;
            Call(categories.changed, new LimitedResourceHandlerArgs { oldAmount = old, newAmount = amount });
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict().SetArgs("amount", amount, "additional-maximum", additionalMaximum);
        }
    }
}
