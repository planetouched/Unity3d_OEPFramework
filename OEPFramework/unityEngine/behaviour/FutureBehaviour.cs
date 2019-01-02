using System;
using OEPFramework.futures;
using OEPFramework.unityEngine.transit;
using OEPFramework.unityEngine._base;

namespace OEPFramework.unityEngine.behaviour
{
    public abstract class FutureBehaviour : Future, IControllable, IDroppableItem, ILoopable
    {
        private readonly int hashCode;

        private readonly ControlLoopTransit controlLoopTransit;
        public event Action<IDroppableItem> onDrop;

        public bool dropped { get { return controlLoopTransit.dropped; } }
        public bool initialized { get { return controlLoopTransit.initialized; } }
        protected FutureBehaviour()
        {
            hashCode = DroppableItemBase.globalHashCode++;
            controlLoopTransit = new ControlLoopTransit();
            controlLoopTransit.onDrop += onDrop;
            controlLoopTransit.onPlay = OnPlay;
            controlLoopTransit.onInitialize = OnInitialize;
            controlLoopTransit.onUninitialize = OnUninitialize;
            controlLoopTransit.onPause = OnPause;

            AddListener(f => { Drop(); });
        }

        protected virtual void OnUninitialize() { }
        protected virtual void OnInitialize() { }
        protected virtual void OnPause() { }
        protected virtual void OnPlay() { }


        public override int GetHashCode()
        {
            return hashCode;
        }

        public void LoopOn(int loopType, Action action)
        {
            controlLoopTransit.LoopOn(loopType, action);
        }

        public void LoopOff(int loopType)
        {
            controlLoopTransit.LoopOff(loopType);
        }

        public void Initialize()
        {
            controlLoopTransit.Initialize();
        }

        public void Uninitialize()
        {
            controlLoopTransit.Uninitialize();
        }

        public void Pause()
        {
            controlLoopTransit.Pause();
        }

        public void Play()
        {
            controlLoopTransit.Play();
        }

        public virtual void Drop()
        {
            if (dropped) return;
            controlLoopTransit.Drop();

            if (onDrop != null)
                onDrop(this);
            onDrop = null;
        }
    }
}
