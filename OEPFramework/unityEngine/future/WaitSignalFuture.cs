using OEPFramework.future;

namespace OEPFramework.unityEngine.future
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
