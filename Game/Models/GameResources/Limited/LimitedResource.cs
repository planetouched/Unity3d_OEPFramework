using System;
using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;
using Basement.BLFramework.Core.Util;
using Basement.Common;

namespace Game.Models.GameResources.Limited
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

        public void ChangeAdditionalMaximum(int addMaximum)
        {
            SetAdditionalMaximum(additionalMaximum + addMaximum);
        }

        public void SetAdditionalMaximum(int setAddtitionalMaximum)
        {
            if (setAddtitionalMaximum < 0)
            {
                throw new Exception("setAdditionalMaximum < 0");
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
