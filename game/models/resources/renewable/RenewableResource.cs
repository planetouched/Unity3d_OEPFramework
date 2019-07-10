using System;
using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;
using Basement.BLFramework.Core.Util;
using Basement.BLFramework.Essential.Time;
using Basement.Common;

namespace Game.Models.Resources.Renewable
{
    public class RenewableResource : ReferenceModelBase<RenewableResourceCategories, RenewableResourceDescription>
    {
        public long lastUpdateTime { get; private set; }
        public int amount { get { return InnerGetAmount(); } }
        private int innerAmount;

        public RenewableResource(RawNode initNode, RenewableResourceCategories categories, RenewableResourceDescription description, IContext context)
            : base(initNode, categories, description, context)
        {
            innerAmount = initNode.GetInt("amount");
            lastUpdateTime = Time.GetUnixTime(initNode.GetLong("ts"));
        }

        public void Change(int addAmount)
        {
            Recount();
            Set(innerAmount + addAmount);
        }

        public void Set(int setAmount)
        {
            if (innerAmount == setAmount) return;

            if (setAmount < 0)
            {
                throw new Exception("value < 0");
            }

            lastUpdateTime = Time.GetUnixTime();
            int oldAmount = innerAmount;
            innerAmount = setAmount > description.maximum ? description.maximum : setAmount;

            Call(categories.changed, new RenewableResourceHandlerArgs { oldAmount = oldAmount, newAmount = innerAmount });
        }

        void Recount()
        {
            if (innerAmount >= description.renewableMaximum) return;
            var currentTime = Time.GetUnixTime();

            var newAmount = innerAmount + (int)(currentTime - lastUpdateTime) / description.recoveryTime * description.recoveryStep;
            lastUpdateTime = currentTime - (currentTime - lastUpdateTime) % description.recoveryTime;

            if (newAmount >= description.renewableMaximum && innerAmount <= description.renewableMaximum)
                newAmount = description.renewableMaximum;

            innerAmount = newAmount;
        }

        int InnerGetAmount()
        {
            int oldAmount = innerAmount;
            Recount();
            if (oldAmount != innerAmount)
            {
                Call(categories.changed, new RenewableResourceHandlerArgs { oldAmount = oldAmount, newAmount = innerAmount });
            }
            return innerAmount;
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict().SetArgs("amount", amount, "ts", lastUpdateTime);
        }
    }
}
