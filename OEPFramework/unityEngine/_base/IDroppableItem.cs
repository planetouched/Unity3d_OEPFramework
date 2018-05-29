using System;

namespace Assets.OEPFramework.unityEngine._base
{
    public interface IDroppableItem
    {
        bool dropped { get; }
        event Action<IDroppableItem> onDrop;
        void Drop();
    }
}
