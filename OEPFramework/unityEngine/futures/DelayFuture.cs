using Assets.OEPFramework.futures;
using OEPFramework.unityEngine;
using OEPFramework.unityEngine.behaviour;

namespace Assets.OEPFramework.unityEngine.futures
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
