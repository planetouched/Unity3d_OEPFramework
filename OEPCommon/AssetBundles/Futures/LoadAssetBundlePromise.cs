using System;
using System.Collections.Generic;
using System.IO;
using Basement.OEPFramework.Futures;
using Basement.OEPFramework.Futures.Coroutine;
using Basement.OEPFramework.UnityEngine;
using Basement.OEPFramework.UnityEngine.Loop;
using UnityEngine;

namespace OEPCommon.AssetBundles.Futures
{
    public class LoadAssetBundlePromise : Future, IProcess
    {
        public AssetBundle assetBundle { get; private set; }

        public float loadingProgress => (GetLoadingProgress() + GetUnpackProgress()) * 0.5f;
        public event Action<IProcess> onProcessComplete;
        public bool isComplete { get; private set; }
        public bool isDependency { get; }

        private readonly string _assetBundleFile;
        private readonly string _storageUrl;
        private UnpackBundlePromise _unpackBundlePromise;
        private readonly bool _async;
        private readonly uint _crc32;
        private readonly string _hash128;
        private bool _isLoad;

        public LoadAssetBundlePromise(string assetBundleFile, string storageUrl, bool isDependency, uint crc32, string hash128, bool async = true)
        {
            SetAsPromise();
            _hash128 = hash128;
            _crc32 = crc32;
            _async = async;
            _assetBundleFile = assetBundleFile;
            _storageUrl = storageUrl;
            this.isDependency = isDependency;
        }

        protected override void OnRun()
        {
            new CoroutineFuture(LoadingProcess).Run();
        }

        private float GetLoadingProgress()
        {
            return _isLoad ? 1 : 0;
        }

        private float GetUnpackProgress()
        {
            return _unpackBundlePromise?.asyncOperationProgress ?? 0;
        }

        private IEnumerator<IFuture> LoadingProcess()
        {
            var fileName = Path.GetFileName(_assetBundleFile);

            byte[] body;
            
            if (!AssetBundleCache.IsCached(fileName, _hash128))
            {
                Debug.Log("Download WWW: " + fileName);
                var request = new AssetBundleWebRequestFuture(_storageUrl, _assetBundleFile, _async, _crc32, Int32.MaxValue);
                yield return request.Run();
                body = request.result;
                AssetBundleCache.AddCache(fileName, _hash128, body, _async);
            }
            else
            {
                Debug.Log("Load from cache: " + fileName);
                var task2FutureGetCache = new UnsafeTask2Future<byte[]>(() => AssetBundleCache.GetCache(fileName, _hash128, _async));
                yield return task2FutureGetCache.Run();
                body = task2FutureGetCache.result;
            }
            
            if (_async)
            {
                var asyncFromMemory = new AssetBundleCreateRequestPromise(() => AssetBundle.LoadFromMemoryAsync(body));
                yield return asyncFromMemory.Run();
                assetBundle = asyncFromMemory.result.assetBundle;
            }
            else
            {
                assetBundle = AssetBundle.LoadFromMemory(body);
            }

            _isLoad = true;

            if (!isDependency)
            {
                _unpackBundlePromise = new UnpackBundlePromise(assetBundle, _async);
                yield return _unpackBundlePromise.Run();

                //check it on new versions. Remove Sync.Add if it works with sync unpacking
                if (_async)
                {
                    assetBundle.Unload(false);
                }
                else
                {
                    Sync.Add(() => { assetBundle.Unload(false); }, Loops.UPDATE);
                }
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