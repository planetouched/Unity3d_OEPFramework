using System;
#if REFVIEW
using OEPFramework.utils;
#endif

namespace OEPFramework.unityEngine._base
{
    public abstract class DroppableItemBase :
#if REFVIEW
        ReferenceCounter,
#endif
        IDroppableItem
    {
        public bool dropped { get; protected set; }
        public event Action<IDroppableItem> onDrop;

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
