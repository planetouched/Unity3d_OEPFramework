using System.Collections.Generic;
using common.logger;
using OEPFramework.unityEngine.behaviour;
using OEPFramework.unityEngine.loop;
using UnityEngine;

namespace game.futures
{
    public class WWWFuture : FutureBehaviour
    {
        public string error { get; private set; }
        public WWW request { get; private set; }

        private readonly string _url;
        private readonly int _tryCount;
        private readonly WWWForm _form;
        private readonly byte[] _data;
        private readonly Dictionary<string, string> _headers;
        private Hash128? _hash128;
        private readonly uint _crc32;

        private int _resendCounter;

        public WWWFuture(string url, WWWForm form, int tryCount = 1)
        {
            _url = url;
            _form = form;
            _tryCount = tryCount;
        }

        public WWWFuture(string url, int tryCount = 1, Hash128? version = null, uint crc32 = 0)
        {
            _url = url;
            _tryCount = tryCount;
            _hash128 = version;
            _crc32 = crc32;
        }

        public WWWFuture(string url, byte[] data, Dictionary<string, string> headers, int tryCount = 1)
        {
            _data = data;
            _headers = headers;
            _url = url;
            _tryCount = tryCount;
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
                Deb.LogWarning("Request error: " + request.error + ", Reload... " + _url);

            if (_resendCounter++ < _tryCount)
            {
                if (_form != null)
                {
                    request = new WWW(_url, _form);
                    return true;
                }

                if (_data != null && _headers != null)
                {
                    request = new WWW(_url, _data, _headers);
                    return true;
                }

                if (_hash128 == null)
                    request = new WWW(_url);
                else
                    request = _crc32 == 0 ? WWW.LoadFromCacheOrDownload(_url, _hash128.Value) : WWW.LoadFromCacheOrDownload(_url, _hash128.Value, _crc32);

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
