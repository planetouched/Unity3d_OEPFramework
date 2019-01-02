using Assets.OEPFramework.futures;
using OEPFramework.unityEngine;

namespace Assets.OEPFramework.unityEngine.futures
{
    public class WaitSignalFuture : Future
    {
        private readonly string signalCategory;
        public WaitSignalFuture(string signalCategory)
        {
            this.signalCategory = signalCategory;
        }

        void SignalComplete(object obj)
        {
            Complete();
        }

        protected override void OnRun()
        {
            GEvent.Attach(signalCategory, SignalComplete, null);
        }

        protected override void OnComplete()
        {
            GEvent.Detach(signalCategory, SignalComplete);
        }
    }
}
