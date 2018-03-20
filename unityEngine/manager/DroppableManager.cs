using System.Collections.Generic;
using OEPFramework.unityEngine._base;

namespace OEPFramework.unityEngine.manager
{
    public static class DroppableManager
    {
        private static readonly HashSet<IDroppableItem> collection = new HashSet<IDroppableItem>();

        public static void Add(IDroppableItem droppable)
        {
            if (droppable.dropped) return;
            collection.Add(droppable);
            droppable.onDrop += Drop;
        }

        private static void Drop(IDroppableItem droppable)
        {
            collection.Remove(droppable);
        }

        public static void Clear()
        {
            foreach (var droppable in new List<IDroppableItem>(collection))
            {
                droppable.onDrop -= Drop;
                droppable.Drop();
            }

            collection.Clear();
        }
    }
}
