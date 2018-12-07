using System;
using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.model;
using Assets.logic.core.util;
using Assets.logic.essential.time;

namespace Assets.game.model.resource.renewableResource
{
    public class RenewableResource : ReferenceModelBase<RenewableResourceDescription>
    {
        public long lastUpdateTime { get; private set; }
        private int innerAmount;

        public RenewableResourceCategories categories { get; private set; }
        public RenewableResourceDescription description { get; private set; }
        public int amount { get { return InnerGetAmount(); } }

        public RenewableResource(RawNode initNode, RenewableResourceDescription description, RenewableResourceCategories categories, IContext context)
            : base(initNode, description, context)
        {
            this.categories = categories;

            innerAmount = initNode.GetInt("amount");
            lastUpdateTime = Time.GetUnixTime(initNode.GetLong("ts"));
        }

        public void Increment(int value)
        {
            Recount();
            Set(innerAmount + value);
        }

        public void Set(int value)
        {
            if (innerAmount == value) return;

            if (value < 0)
            {
                throw new Exception("value < 0");
            }

            lastUpdateTime = Time.GetUnixTime();
            int oldAmount = innerAmount;
            innerAmount = value > description.maximum ? description.maximum : value;

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
            return SerializeUtil.Dict("amount", amount, "ts", lastUpdateTime);
        }
    }
}
