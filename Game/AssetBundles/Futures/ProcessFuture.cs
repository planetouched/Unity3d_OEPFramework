using System;
using Basement.OEPFramework.Futures;

namespace Game.AssetBundle.Futures
{
    public class ProcessFuture : Future, IProcess
    {
        public float loadingProgress => _process.loadingProgress;
        public float unpackProgress => _process.unpackProgress;
        public event Action<IProcess> onProcessComplete;
        public bool isComplete { get; private set; }

        private readonly IProcess _process;

        public ProcessFuture(IProcess process)
        {
            _process = process;
        }

        protected override void OnRun()
        {
            _process.onProcessComplete += _ =>
            {
                Complete();
            };
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
