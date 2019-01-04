using System;
using System.Collections.Generic;
using game.futures;
using OEPFramework.futures;
using OEPFramework.unityEngine.utils;
using UnityEngine;

namespace game.assetBundle.futures
{
    public class LoadBundlePromise : Future, IProcess
    {
        public AssetBundle assetBundle { get; private set; }
        public WWW request { get; private set; }
        
        private readonly string _assetBundleName;
        private readonly string _url;
        private WWWFuture _wwwFuture;
        private UnpackBundlePromise _unpackBundlePromise;
        private readonly bool _async;
        readonly Hash128? _version;
        private readonly uint _crc32;

        public float loadingProgress => _wwwFuture.request.progress;
        public float unpackProgress => _unpackBundlePromise.asyncOperation.progress;
        public Action<IProcess> onProcessComplete { get; set; }
        public bool isComplete { get; private set; }
        public bool dependency { get; }

        public LoadBundlePromise(string assetBundleName, string url, bool dependency, bool async = true, Hash128? version = null, uint crc32 = 0)
        {
            SetAsPromise();
            _crc32 = crc32;
            _version = version;
            _async = async;
            _assetBundleName = assetBundleName;
            _version = version;
            _url = url;
            this.dependency = dependency;
        }

        protected override void OnRun()
        {
            FutureUtils.Coroutine(LoadingProcess).Run();
        }

        private IEnumerator<IFuture> LoadingProcess()
        {
            Debug.Log("AssetBundle load: " + _url + _assetBundleName);
            _wwwFuture = new WWWFuture(_url + _assetBundleName, Int32.MaxValue, _version, _crc32);
            yield return _wwwFuture.Run();

            assetBundle = _wwwFuture.request.assetBundle;

            if (!dependency)
            {
                _unpackBundlePromise = new UnpackBundlePromise(assetBundle, _async);
                yield return _unpackBundlePromise.Run();
                assetBundle.Unload(false);
                _wwwFuture.request.Dispose();
            }
            else
            {
                request = _wwwFuture.request;
            }

            Complete();
        }

        public UnityEngine.Object[] GetAssets()
        {
            if (!dependency)
                return _unpackBundlePromise.allAssets;

            return null;
        }

        protected override void OnComplete()
        {
            isComplete = true;
            if (onProcessComplete != null)
                onProcessComplete(this);
            onProcessComplete = null;
        }
    }
}
