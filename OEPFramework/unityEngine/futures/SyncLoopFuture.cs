using Assets.OEPFramework.futures;
using OEPFramework.unityEngine;

namespace Assets.OEPFramework.unityEngine.futures
{
    public class SyncLoopFuture : Future
    {
        private readonly int loopType;

        public SyncLoopFuture(int loopType)
        {
            this.loopType = loopType;
        }

        protected override void OnRun()
        {
            Sync.Add(Complete, loopType);
        }

        protected override void OnComplete()
        {
        }
    }
}
