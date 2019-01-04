using OEPFramework.futures;
using OEPFramework.unityEngine.behaviour;

namespace OEPFramework.unityEngine.futures
{
    public class WaitFuture : Future, IPlayable
    {
        private readonly float _sec;
        private Timer _waitTimer;
        
        public WaitFuture(float sec)
        {
            _sec = sec;
        }

        protected override void OnRun()
        {
            _waitTimer = Timer.Create(_sec, Complete, null, true);
        }

        protected override void OnComplete()
        {
            if (_waitTimer != null)
                _waitTimer.Drop();
        }

        public void Pause()
        {
            if (_waitTimer != null)
                _waitTimer.Pause();
        }

        public void Play()
        {
            if (_waitTimer != null)
                _waitTimer.Resume();
        }
    }
}
