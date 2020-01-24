using System;
using Basement.OEPFramework.Futures;

namespace OEPCommon.AssetBundles.Futures
{
    public class ProcessFuture : Future, IProcess
    {
        public float loadingProgress => _process.loadingProgress;
        public event Action<IProcess> onProcessComplete;
        public bool isComplete { get; private set; }

        private readonly IProcess _process;

        public ProcessFuture(IProcess process)
        {
            _process = process;
        }

        protected override void OnRun()
        {
            if (_process.isComplete)
            {
                Complete();
                return;
            }
            
            _process.onProcessComplete += OnProcessComplete;
        }

        private void OnProcessComplete(IProcess _)
        {
            Complete();
        }

        protected override void OnComplete()
        {
            _process.onProcessComplete -= OnProcessComplete;
            
            if (isDone)
            {
                isComplete = true;
                onProcessComplete?.Invoke(this);
                onProcessComplete = null;
            }
        }
    }
}
