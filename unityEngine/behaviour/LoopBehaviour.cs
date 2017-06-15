using System;
using OEPFramework.unityEngine.loop;
using OEPFramework.unityEngine._base;

namespace OEPFramework.unityEngine.behaviour
{
    public abstract class LoopBehaviour : DroppableItemBase, ILoopable
    {
        public bool callActions { get; protected set; }

        private readonly Action[] actions;
        private readonly int[] orders;

        protected LoopBehaviour()
        {
            actions = new Action[EngineLoopManager.LoopsCount()];
            orders = new int[EngineLoopManager.LoopsCount()];
            callActions = true;
        }

        public void LoopOn(int loopType, Action action)
        {
            if (dropped)
                throw new Exception("Dropped");

            if (actions[loopType] == null)
            {
                EngineLoopManager.GetEngineLoop(loopType).AddToLast(this);
                actions[loopType] = action;
            }
        }

        public void SetOrder(int loopType, int order)
        {
            orders[loopType] = order;
        }

        public int GetOrder(int loopType)
        {
            return orders[loopType];
        }

        public void ExecuteAction(int loopType)
        {
            if (actions[loopType] != null)
                actions[loopType]();
        }

        public void LoopOff(int loopType)
        {
            if (actions[loopType] != null)
            {
                EngineLoopManager.GetEngineLoop(loopType).Remove(this);
                actions[loopType] = null;
            }
        }

        /*
        public void SetIndexToLast(int loopType)
        {
            EngineLoopManager.GetEngineLoop(loopType).Remove(this);
            EngineLoopManager.GetEngineLoop(loopType).AddToLast(this);
        }
        public void SwapWith(LoopBehaviour target, int loopType)
        {
            throw new NotImplementedException();
        }
        public void SetIndexToFirst(int loopType)
        {
            throw new NotImplementedException();
        }*/

        public override void Drop()
        {
            if (dropped) return;
            callActions = false;

            for (int i = 0; i < actions.Length; i++)
                LoopOff(i);

            base.Drop();
        }
    }
}