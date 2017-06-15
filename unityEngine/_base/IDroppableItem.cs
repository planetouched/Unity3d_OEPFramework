using System;

namespace OEPFramework.unityEngine._base
{
    public interface IDroppableItem
    {
        bool dropped { get; }
        event Action onDrop;
        void Drop();
    }
}
