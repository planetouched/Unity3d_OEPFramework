using System;
using OEPFramework.unityEngine.behaviour;
using OEPFramework.unityEngine.loop;

namespace game.assetBundle.futures
{
    public class ProcessFuture : FutureBehaviour, IProcess
    {
        public float loadingProgress => _process.loadingProgress;
        public float unpackProgress => _process.unpackProgress;
        public Action<IProcess> onProcessComplete { get; set; }
        public bool isComplete { get; private set; }

        private readonly IProcess _process;

        public ProcessFuture(IProcess process)
        {
            _process = process;
            LoopOn(Loops.UPDATE, Update);
        }

        private void Update()
        {
            if (_process.isComplete)
                Complete();
        }

        protected override void OnRun()
        {
            Play();
        }

        protected override void OnComplete()
        {
            if (isDone)
            {
                isComplete = true;
                if (onProcessComplete != null)
                    onProcessComplete(this);
                onProcessComplete = null;
            }
        }
    }
}
