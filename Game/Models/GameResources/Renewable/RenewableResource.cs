using System;
using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;
using Basement.BLFramework.Core.Util;
using Basement.BLFramework.Essential.Time;
using Basement.Common;

namespace Game.Models.GameResources.Renewable
{
    public class RenewableResource : ReferenceModelBase<RenewableResourceCategories, RenewableResourceDescription>
    {
        public long lastUpdateTime { get; private set; }

        public int amount {
            get
            {
                Recount();
                return _innerAmount;
            }
        }
        
        private int _innerAmount;

        public RenewableResource(RawNode initNode, RenewableResourceCategories categories, RenewableResourceDescription description, IContext context)
            : base(initNode, categories, description, context)
        {
            _innerAmount = initNode.GetInt("amount");
            lastUpdateTime = TimeUtil.GetUnixTime(initNode.GetLong("ts"));
        }

        public void Change(int addAmount)
        {
            Recount();
            InnerSet(_innerAmount + addAmount, _innerAmount >= description.renewableMaximum);
        }

        public void Set(int setAmount)
        {
            InnerSet(setAmount, true);
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict().SetArgs("amount", amount, "ts", lastUpdateTime);
        }
        
        public void Recount()
        {
            if (_innerAmount >= description.renewableMaximum) return;
            
            var currentTime = TimeUtil.GetUnixTime();

            var newAmount = _innerAmount + (int)(currentTime - lastUpdateTime) / description.recoveryTime * description.recoveryStep;

            lastUpdateTime = currentTime - (currentTime - lastUpdateTime) % description.recoveryTime;

            if (newAmount >= description.renewableMaximum && _innerAmount <= description.renewableMaximum)
            {
                newAmount = description.renewableMaximum;
            }

            int oldAmount = _innerAmount;
            
            _innerAmount = newAmount;
            
            if (oldAmount != _innerAmount)
            {
                Call(categories.changed, new RenewableResourceHandlerArgs { oldAmount = oldAmount, newAmount = _innerAmount });
            }
        }

        private void InnerSet(int setAmount, bool updateTime)
        {
            if (_innerAmount == setAmount) return;

            if (setAmount < 0)
            {
                throw new Exception("value < 0");
            }

            int oldAmount = _innerAmount;
            _innerAmount = setAmount > description.maximum ? description.maximum : setAmount;

            if (updateTime)
            {
                lastUpdateTime = TimeUtil.GetUnixTime();
            }

            Call(categories.changed, new RenewableResourceHandlerArgs { oldAmount = oldAmount, newAmount = _innerAmount });
        }
    }
}
