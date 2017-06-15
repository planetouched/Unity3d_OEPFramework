using OEPFramework.common.future;

namespace OEPFramework.common.service.future
{
    public class ManualStateFuture : ThreadSafeFuture
    {
        public int manualState { get; private set; }

        public void SetState(int state)
        {
            manualState = state;
        }
        protected override void OnRun()
        {
        }

        protected override void OnComplete()
        {
        }
    }
}
