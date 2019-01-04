using System;
using OEPFramework.unityEngine.loop;
using OEPFramework.unityEngine._base;

namespace OEPFramework.unityEngine.behaviour
{
    public abstract class LoopBehaviour : DroppableItemBase, ILoopable
    {
        public bool callActions { get; protected set; }

        private readonly Action[] _actions;
        private readonly int[] _orders;

        protected LoopBehaviour()
        {
            _actions = new Action[EngineLoopManager.LoopsCount()];
            _orders = new int[EngineLoopManager.LoopsCount()];
            callActions = true;
        }

        public void LoopOn(int loopType, Action action)
        {
            if (dropped)
                throw new Exception("Dropped");

            if (_actions[loopType] == null)
            {
                EngineLoopManager.GetEngineLoop(loopType).AddToLast(this);
                _actions[loopType] = action;
            }
        }

        public void SetOrder(int loopType, int order)
        {
            _orders[loopType] = order;
        }

        public int GetOrder(int loopType)
        {
            return _orders[loopType];
        }

        public void ExecuteAction(int loopType)
        {
            if (_actions[loopType] != null)
                _actions[loopType]();
        }

        public void LoopOff(int loopType)
        {
            if (_actions[loopType] != null)
            {
                EngineLoopManager.GetEngineLoop(loopType).Remove(this);
                _actions[loopType] = null;
            }
        }

        public override void Drop()
        {
            if (dropped) return;
            callActions = false;

            for (int i = 0; i < _actions.Length; i++)
                LoopOff(i);

            base.Drop();
        }
    }
}