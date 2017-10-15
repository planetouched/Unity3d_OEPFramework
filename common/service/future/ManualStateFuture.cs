using OEPFramework.common.future;

namespace OEPFramework.common.service.future
{
    public class ManualStateFuture : ThreadSafeFuture
    {
        public int state { get; private set; }

        public void SetState(int state)
        {
            this.state = state;
        }
        protected override void OnRun()
        {
        }

        protected override void OnComplete()
        {
        }
    }
}
