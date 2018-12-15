using System.Collections.Generic;
using OEPFramework.unityEngine.behaviour;

namespace OEPFramework.unityEngine.loop
{
    public class EngineLoop
    {
        internal class InnerComparer : IComparer<LoopBehaviour>
        {
            private readonly int loopType;

            public InnerComparer(int loopType)
            {
                this.loopType = loopType;
            }

            public int Compare(LoopBehaviour x, LoopBehaviour y)
            {
                if (x.GetOrder(loopType) > y.GetOrder(loopType)) return 1;
                if (x.GetOrder(loopType) < y.GetOrder(loopType)) return -1;
                return 0;
            }
        }

        private readonly InnerComparer comparer;

        public List<LoopBehaviour> items = new List<LoopBehaviour>();
        readonly List<LoopBehaviour> toAdd = new List<LoopBehaviour>();
        readonly List<LoopBehaviour> toRemove = new List<LoopBehaviour>();

        private int behaviourOrder;
        private readonly int loopType;

        public EngineLoop(int loopType)
        {
            comparer = new InnerComparer(loopType);
            this.loopType = loopType;
        }

        public void AddToLast(LoopBehaviour behaviour)
        {
            toAdd.Add(behaviour);
        }

        public void Remove(LoopBehaviour behaviour)
        {
            int idx = toAdd.IndexOf(behaviour);
            if (idx != -1)
                toAdd.RemoveAt(idx);
            else
                toRemove.Add(behaviour);
        }

        public void CallAllBehavioursActions()
        {
            Sync.Process(loopType);
            
            //если были добавлены из циклов другого типа
            ModifyIfNeeded();

            //вызов существующих циклов
            InnerCall(items);

            //вызов новых добавленых циклов
            for (;;)
            {
                var newLoops = ModifyIfNeeded();
                if (newLoops != null)
                {
                    InnerCall(newLoops);
                }
                else
                {
                    break;
                }
            }
        }

        void InnerCall(List<LoopBehaviour> loops)
        {
            for (int i = 0; i < loops.Count; i++)
            {
                var current = loops[i];
                if (current != null && (!current.dropped && current.callActions))
                    current.ExecuteAction(loopType);
            }
        }

        public List<LoopBehaviour> ModifyIfNeeded()
        {
            if (toRemove.Count > 0)
            {
                foreach (var behaviour in toRemove)
                {
                    int idx = GetIndex(behaviour);
                    if (idx >= 0)
                        items.RemoveAt(idx);
                }

                toRemove.Clear();
            }

            if (toAdd.Count > 0)
            {
                foreach (var behaviour in toAdd)
                {
                    items.Add(behaviour);
                    behaviour.SetOrder(loopType, behaviourOrder++);
                }

                var newLoops = new List<LoopBehaviour>(toAdd);
                toAdd.Clear();
                return newLoops;
            }

            return null;
        }

        public int GetIndex(LoopBehaviour behaviour)
        {
            return items.BinarySearch(behaviour, comparer);
        }
    }
}