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
        private int _innerAmount;
        
        public long LastUpdateTime { get; private set; }
        
        public int Amount 
        {
            get
            {
                Recount();
                return _innerAmount;
            }
        }

        public RenewableResource(RawNode initNode, RenewableResourceCategories categories, RenewableResourceDescription description, IContext context)
            : base(initNode, categories, description, context)
        {
            _innerAmount = initNode.GetInt("amount");
            LastUpdateTime = TimeUtil.GetUnixTime(initNode.GetLong("ts"));
        }

        public void BackInTime(long deltaTime)
        {
            LastUpdateTime -= deltaTime;
            Recount();
        }

        public void Change(int addAmount)
        {
            Recount();
            InnerSet(_innerAmount + addAmount, _innerAmount >= description.RenewableMaximum);
        }

        public void Set(int setAmount)
        {
            InnerSet(setAmount, true);
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict().SetArgs("amount", Amount, "ts", LastUpdateTime);
        }
        
        public void Recount()
        {
            if (_innerAmount >= description.RenewableMaximum) return;
            
            var currentTime = TimeUtil.GetUnixTime();

            var newAmount = _innerAmount + (int)(currentTime - LastUpdateTime) / description.RecoveryTime * description.RecoveryStep;

            LastUpdateTime = currentTime - (currentTime - LastUpdateTime) % description.RecoveryTime;

            if (newAmount >= description.RenewableMaximum && _innerAmount <= description.RenewableMaximum)
            {
                newAmount = description.RenewableMaximum;
            }

            int oldAmount = _innerAmount;
            
            _innerAmount = newAmount;
            
            if (oldAmount != _innerAmount)
            {
                Call(categories.Changed, new RenewableResourceHandlerArgs { OldAmount = oldAmount, NewAmount = _innerAmount });
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
            _innerAmount = setAmount > description.Maximum ? description.Maximum : setAmount;

            if (updateTime)
            {
                LastUpdateTime = TimeUtil.GetUnixTime();
            }

            Call(categories.Changed, new RenewableResourceHandlerArgs { OldAmount = oldAmount, NewAmount = _innerAmount });
        }
    }
}
