using OEPFramework.future;
using OEPFramework.unityEngine.behaviour;

namespace OEPFramework.unityEngine.future
{
    public class DelayFuture : Future, IPlayable
    {
        private readonly float delay;
        private readonly IFuture delayFuture;
        private Timer timer;
        public DelayFuture(float delay, IFuture delayFuture)
        {
            this.delay = delay;
            this.delayFuture = delayFuture;
        }
        protected override void OnRun()
        {
            delayFuture.AddListener(f =>
            {
                if (f.isDone)
                    Complete();
            });

            timer = Timer.Create(delay, () =>
            {
                delayFuture.Run();
            }, null, true);
        }

        protected override void OnComplete()
        {
            if (isCancelled)
            {
                delayFuture.Cancel();
            }

            if (timer != null)
                timer.Drop();
        }

        public void Pause()
        {
            if (timer != null)
                timer.Pause();
        }

        public void Play()
        {
            if (timer != null)
                timer.Resume();
        }
    }
}
