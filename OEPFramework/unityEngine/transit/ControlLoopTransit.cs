using System;
using Assets.OEPFramework.unityEngine.behaviour;

namespace Assets.OEPFramework.unityEngine.transit
{
    public class ControlLoopTransit : ControlLoopBehaviour
    {
        public Action onUninitialize;
        public Action onInitialize;
        public Action onPause;
        public Action onPlay;

        protected override void OnInitialize()
        {
            if (onInitialize != null)
                onInitialize();
        }

        protected override void OnPause()
        {
            if (onPause != null)
                onPause();
        }

        protected override void OnPlay()
        {
            if (onPlay != null)
                onPlay();
        }

        protected override void OnUninitialize()
        {
            if (onUninitialize != null)
                onUninitialize();
            base.OnUninitialize();
        }

        public override void Drop()
        {
            if (dropped) return;
            onUninitialize = null;
            onInitialize = null;
            onPause = null;
            onPlay = null;
            base.Drop();
        }
    }
}
