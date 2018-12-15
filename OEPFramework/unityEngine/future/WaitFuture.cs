using OEPFramework.future;
using OEPFramework.unityEngine.behaviour;

namespace OEPFramework.unityEngine.future
{
    public class WaitFuture : Future, IPlayable
    {
        private readonly float sec;
        private Timer waitTimer;
        public WaitFuture(float sec)
        {
            this.sec = sec;
        }

        protected override void OnRun()
        {
            waitTimer = Timer.Create(sec, Complete, null, true);
        }

        protected override void OnComplete()
        {
            if (waitTimer != null)
                waitTimer.Drop();
        }

        public void Pause()
        {
            if (waitTimer != null)
                waitTimer.Pause();
        }

        public void Play()
        {
            if (waitTimer != null)
                waitTimer.Resume();
        }
    }
}
