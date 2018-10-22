using System.Collections.Generic;
using Assets.OEPFramework.unityEngine.behaviour;
using Assets.OEPFramework.unityEngine.loop;
using UnityEngine;

namespace Assets.game.furure
{
    public class WWWFuture : FutureBehaviour
    {
        public string error { get; private set; }
        public WWW request { get; private set; }

        private readonly string url;
        private readonly int tryCount;
        private readonly WWWForm form;
        private readonly byte[] data;
        private readonly Dictionary<string, string> headers;
        private Hash128? hash128;
        private readonly uint crc32;

        private int resendCounter;

        public WWWFuture(string url, WWWForm form, int tryCount = 1)
        {
            this.url = url;
            this.form = form;
            this.tryCount = tryCount;
        }

        public WWWFuture(string url, int tryCount = 1, Hash128? version = null, uint crc32 = 0)
        {
            this.url = url;
            this.tryCount = tryCount;
            hash128 = version;
            this.crc32 = crc32;
        }

        public WWWFuture(string url, byte[] data, Dictionary<string, string> headers, int tryCount = 1)
        {
            this.data = data;
            this.headers = headers;
            this.url = url;
            this.tryCount = tryCount;
        }

        private void Update()
        {
            if (!request.isDone) return;
            error = request.error;

            if (string.IsNullOrEmpty(error))
            {
                Complete();
                return;
            }

            if (!string.IsNullOrEmpty(error) && !Request())
            {
                Cancel();
            }
        }

        bool Request()
        {
            if (request != null)
                Debug.LogWarning("Request error: " + request.error + ", Reload... " + url);

            if (resendCounter++ < tryCount)
            {
                if (form != null)
                {
                    request = new WWW(url, form);
                    return true;
                }

                if (data != null && headers != null)
                {
                    request = new WWW(url, data, headers);
                    return true;
                }

                if (hash128 == null)
                    request = new WWW(url);
                else
                    request = crc32 == 0 ? WWW.LoadFromCacheOrDownload(url, hash128.Value) : WWW.LoadFromCacheOrDownload(url, hash128.Value, crc32);

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
