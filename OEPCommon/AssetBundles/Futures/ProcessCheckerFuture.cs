using Basement.OEPFramework.Futures;

namespace OEPCommon.AssetBundles.Futures
{
    public class ProcessCheckerFuture : Future
    {
        private readonly ProcessChecker _processChecker;
        private readonly Unloader _unloader;
        private readonly int _simultaneousLimit;
        
        public ProcessCheckerFuture(ProcessChecker processChecker, Unloader unloader = null, int simultaneousLimit = int.MaxValue)
        {
            _simultaneousLimit = simultaneousLimit;
            _unloader = unloader;
            _processChecker = processChecker;
        }
        
        protected override void OnRun()
        {
            _processChecker.onProcessComplete += ProcessChecker_Complete;
            _processChecker.Load(_unloader, _simultaneousLimit);
        }

        private void ProcessChecker_Complete(IProcess obj)
        {
            Complete();
        }

        protected override void OnComplete()
        {
            if (isCancelled)
            {
                _processChecker.Cancel();
            }
        }
    }
}