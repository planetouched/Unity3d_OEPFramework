using System;

namespace OEPFramework.unityEngine._base
{
    public interface IDroppableItem
    {
        event Action<IDroppableItem> onDrop;
        void Drop();
    }
}
