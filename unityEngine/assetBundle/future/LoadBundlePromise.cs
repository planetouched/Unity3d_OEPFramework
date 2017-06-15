using System;
using System.Collections.Generic;
using OEPFramework.common;
using OEPFramework.common.future;
using OEPFramework.unityEngine.future;
using OEPFramework.unityEngine.utils;
using UnityEngine;

namespace OEPFramework.unityEngine.assetBundle.future
{
    public class LoadBundlePromise : Future, IProcess
    {
        private readonly string assetBundleName;
        private readonly int version;
        private readonly string url;

        private WWWFuture wwwFuture;
        private UnpackBundlePromise unpackBundlePromise;
        public AssetBundle assetBundle { get; private set; }
        public WWW request { get; private set; }
        private readonly bool async;

        public float loadingProgress { get { return wwwFuture.request.progress; } }
        public float unpackProgress { get { return unpackBundlePromise.asyncOperation.progress; } }
        public Action<IProcess> onProcessComplete { get; set; }
        public bool isComplete { get; private set; }
        public bool dependency { get; private set; }

        public LoadBundlePromise(string assetBundleName, string url, bool dependency, int version = 0, bool async = true)
        {
            SetAsPromise();
            this.async = async;
            this.assetBundleName = assetBundleName;
            this.version = version;
            this.url = url;
            this.dependency = dependency;
        }

        protected override void OnRun()
        {
            FutureUtils.Coroutine(LoadingProcess).Run();
        }

        private IEnumerator<IFuture> LoadingProcess()
        {
            Debug.Log("AssetBundle load: " + url + assetBundleName);
            wwwFuture = new WWWFuture(url + assetBundleName, version, Int32.MaxValue);
            yield return wwwFuture.Run();

            assetBundle = wwwFuture.request.assetBundle;

            if (!dependency)
            {
                unpackBundlePromise = new UnpackBundlePromise(assetBundle, async);
                yield return unpackBundlePromise.Run();
                assetBundle.Unload(false);
                wwwFuture.request.Dispose();
            }
            else
            {
                request = wwwFuture.request;
            }

            Complete();
        }

        public UnityEngine.Object[] GetAssets()
        {
            if (!dependency)
                return unpackBundlePromise.allAssets;

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
