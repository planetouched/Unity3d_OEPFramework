using OEPFramework.futures;
using UnityEngine;
using UnityEngine.Networking;

namespace game.assetBundle.futures
{
    public class UnityWebRequestAssetBundleFuture : Future
    {
        public UnityWebRequest request { get; private set; }

        private AsyncOperation _asyncOperation;
        private readonly Hash128 _hash;
        private readonly uint _crc32;
        private readonly string _uri;
        private readonly int _attemptsCount;
        private int _resendCounter;
        
        public UnityWebRequestAssetBundleFuture(string uri, Hash128 hash, uint crc32, int attemptsCount = 0)
        {
            _crc32 = crc32;
            _hash = hash;
            _uri = uri;
            _attemptsCount = attemptsCount;
        }
        
        protected override void OnRun()
        {
            SendRequest();
        }

        private void SendRequest()
        {
            request?.Dispose();
            request = CreateRequest();
            _asyncOperation = request.SendWebRequest();
            _asyncOperation.completed += AsyncOperationOnCompleted;
        }

        private void AsyncOperationOnCompleted(AsyncOperation asyncOperation)
        {
            if (request.isHttpError || !string.IsNullOrEmpty(request.error))
            {
                if (_resendCounter < _attemptsCount)
                {
                    _resendCounter++;
                    SendRequest();
                }
                else
                {
                    Cancel();
                }
            }
            else
            {
                Complete();
            }
        }

        private UnityWebRequest CreateRequest()
        {
            return UnityWebRequestAssetBundle.GetAssetBundle(_uri, _hash, _crc32);
        }

        protected override void OnComplete()
        {
            if (isCancelled)
            {
                request.Dispose();
            }
        }
    }
}