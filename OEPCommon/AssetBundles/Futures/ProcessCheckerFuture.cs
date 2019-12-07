using Basement.OEPFramework.Futures;

namespace OEPCommon.AssetBundles.Futures
{
    public class ProcessCheckerFuture : Future
    {
        private readonly ProcessChecker _processChecker;
        private readonly Unloader _unloader;
        
        public ProcessCheckerFuture(ProcessChecker processChecker, Unloader unloader = null)
        {
            _unloader = unloader;
            _processChecker = processChecker;
        }
        
        protected override void OnRun()
        {
            _processChecker.onProcessComplete += ProcessChecker_Complete;
            _processChecker.Load(_unloader);
        }

        private void ProcessChecker_Complete(IProcess obj)
        {
            Complete();
        }

        protected override void OnComplete()
        {
            if (isCancelled)
            {
                _processChecker.Drop();
            }
        }
    }
}