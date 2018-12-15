using System;
using common;
using logic.core.context;
using logic.core.model;
using logic.core.util;
using logic.essential.time;

namespace game.model.resource.renewable
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
            innerAmount = setAmount > GetDescription().maximum ? GetDescription().maximum : setAmount;

            Call(categories.changed, new RenewableResourceHandlerArgs { oldAmount = oldAmount, newAmount = innerAmount });
        }

        void Recount()
        {
            if (innerAmount >= GetDescription().renewableMaximum) return;
            var currentTime = Time.GetUnixTime();

            var newAmount = innerAmount + (int)(currentTime - lastUpdateTime) / GetDescription().recoveryTime * GetDescription().recoveryStep;
            lastUpdateTime = currentTime - (currentTime - lastUpdateTime) % GetDescription().recoveryTime;

            if (newAmount >= GetDescription().renewableMaximum && innerAmount <= GetDescription().renewableMaximum)
                newAmount = GetDescription().renewableMaximum;

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
