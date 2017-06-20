using OEPFramework.unityEngine.behaviour;
using OEPFramework.unityEngine.loop;
using UnityEngine;

namespace OEPFramework.unityEngine.future
{
    public class WWWFuture : FutureBehaviour
    {
        public string error { get; private set; }
        public WWW request { get; private set; }

        private readonly string url;
        private readonly int tryCount;
        private int resendCounter;
        private readonly int version;

        public WWWFuture(string url, int version = 0, int tryCount = 1)
        {
            this.version = version;
            this.url = url;
            this.tryCount = tryCount;
        }

        private void Update()
        {
            if (!request.isDone) return;
            error = request.error;
            if (error == null || (error != null && !Request()))
                Complete();
        }

        bool Request()
        {
            if (request != null)
                Debug.LogWarning("Request error: " + request.error + ", Reload... " + url);

            if (resendCounter++ < tryCount)
            {
                request = version == 0 ? new WWW(url) : WWW.LoadFromCacheOrDownload(url, version);
                return true;
            }

            return false;
        }

        protected override void OnRun()
        {
            Request();
            LoopOn(Loops.UPDATE, Update);
            Play();
        }

        protected override void OnComplete()
        {
        }
    }
}
