using System;
using OEPFramework.unityEngine.behaviour;
using OEPFramework.unityEngine.loop;

namespace game.assetBundle.futures
{
    public class ProcessFuture : FutureBehaviour, IProcess
    {
        public float loadingProgress => _process.loadingProgress;
        public float unpackProgress => _process.unpackProgress;
        public event Action<IProcess> onProcessComplete;
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
                onProcessComplete?.Invoke(this);
                onProcessComplete = null;
            }
        }
    }
}
