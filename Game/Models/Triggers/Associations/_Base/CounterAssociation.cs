using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Util;
using Basement.Common;

namespace Game.Models.Triggers.Associations._Base
{
    public abstract class CounterAssociation : Association
    {
        private const string CountKey = "count";
        
        public int count { get; protected set; }
        public int maxCount { get; }
        
        protected CounterAssociation(RawNode initNode, RawNode descriptionNode, AssociationCategories categories, IContext context) : base(initNode, descriptionNode, categories, context)
        {
            count = initNode.GetInt(CountKey);
            maxCount = descriptionNode.GetInt("max-count");
        }

        protected void Increment(int addCount)
        {
            int oldCount = count;
            count += addCount;
            
            Call(categories.changed, new CounterAssociationHandlerArgs {oldCount = oldCount, newCount = count});
            
            if (count >= maxCount)
            {
                Complete();
            }
        }

        public override void Activate()
        {
            count = 0;
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict(base.Serialize()).SetArgs(CountKey, count);
        }
    }
}