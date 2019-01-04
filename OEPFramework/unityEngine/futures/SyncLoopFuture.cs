using OEPFramework.futures;

namespace OEPFramework.unityEngine.futures
{
    public class SyncLoopFuture : Future
    {
        private readonly int _loopType;

        public SyncLoopFuture(int loopType)
        {
            _loopType = loopType;
        }

        protected override void OnRun()
        {
            Sync.Add(Complete, _loopType);
        }

        protected override void OnComplete()
        {
        }
    }
}
