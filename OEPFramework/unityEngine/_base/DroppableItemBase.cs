using System;

#if REFVIEW
using OEPFramework.utils;
#endif

namespace Assets.OEPFramework.unityEngine._base
{
    public abstract class DroppableItemBase :
#if REFVIEW
        ReferenceCounter,
#endif
        IDroppableItem
    {
        public static int globalHashCode;

        private readonly int hashCode;
        public bool dropped { get; protected set; }
        public event Action<IDroppableItem> onDrop;

        protected DroppableItemBase()
        {
            hashCode = globalHashCode++;
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        public virtual void Drop()
        {
            if (dropped) return;
            dropped = true;

            if (onDrop != null)
                onDrop(this);
            onDrop = null;
        }
    }
}
