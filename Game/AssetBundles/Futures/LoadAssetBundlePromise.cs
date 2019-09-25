using System;
using System.Collections.Generic;
using Basement.OEPFramework.Futures;
using Basement.OEPFramework.UnityEngine.Util;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.AssetBundle.Futures
{
    public class LoadAssetBundlePromise : Future, IProcess
    {
        public UnityEngine.AssetBundle assetBundle { get; private set; }
        public UnityWebRequest request { get; private set; } 
        public float loadingProgress => _loadFuture.request.downloadProgress;
        public float unpackProgress => _unpackBundlePromise.asyncOperation.progress;
        public event Action<IProcess> onProcessComplete;
        public bool isComplete { get; private set; }
        public bool isDependency { get; }
        
        private readonly string _assetBundleName;
        private readonly string _url;
        private UnityWebRequestAssetBundleFuture _loadFuture;
        private UnpackBundlePromise _unpackBundlePromise;
        private readonly bool _async;
        private readonly Hash128 _hash;
        private readonly uint _crc32;

        public LoadAssetBundlePromise(string assetBundleName, string url, bool isDependency, Hash128 hash, uint crc32, bool async = true)
        {
            SetAsPromise(); 
            _crc32 = crc32;
            _hash = hash;
            _async = async;
            _assetBundleName = assetBundleName;
            _hash = hash;
            _url = url;
            this.isDependency = isDependency;
        }

        protected override void OnRun()
        {
            FutureUtils.Coroutine(LoadingProcess).Run();
        }

        private IEnumerator<IFuture> LoadingProcess()
        {
            Debug.Log("AssetBundle load: " + _url + _assetBundleName);
            
            _loadFuture = new UnityWebRequestAssetBundleFuture(_url + _assetBundleName, _hash, _crc32, Int32.MaxValue);
            yield return _loadFuture.Run();

            assetBundle = DownloadHandlerAssetBundle.GetContent(_loadFuture.request);

            if (!isDependency)
            {
                _unpackBundlePromise = new UnpackBundlePromise(assetBundle, _async);
                yield return _unpackBundlePromise.Run();
                assetBundle.Unload(false);
                _loadFuture.request.Dispose();
            }
            else
            {
                request = _loadFuture.request;
            }

            Complete();
        }

        public UnityEngine.Object[] GetAssets()
        {
            if (!isDependency)
            {
                return _unpackBundlePromise.allAssets;
            }

            return null;
        }

        protected override void OnComplete()
        {
            isComplete = true;
            onProcessComplete?.Invoke(this);
            onProcessComplete = null;
        }
    }
}
