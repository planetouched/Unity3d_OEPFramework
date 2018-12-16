﻿using System;
using System.Collections.Generic;
using game.future;
using OEPFramework.future;
using OEPFramework.unityEngine.utils;
using UnityEngine;

namespace game.assetBundle.future
{
    public class LoadBundlePromise : Future, IProcess
    {
        private readonly string assetBundleName;
        private readonly string url;

        private WWWFuture wwwFuture;
        private UnpackBundlePromise unpackBundlePromise;
        public AssetBundle assetBundle { get; private set; }
        public WWW request { get; private set; }
        private readonly bool async;
        readonly Hash128? version;
        private readonly uint crc32;

        public float loadingProgress { get { return wwwFuture.request.progress; } }
        public float unpackProgress { get { return unpackBundlePromise.asyncOperation.progress; } }
        public Action<IProcess> onProcessComplete { get; set; }
        public bool isComplete { get; private set; }
        public bool dependency { get; private set; }

        public LoadBundlePromise(string assetBundleName, string url, bool dependency, bool async = true, Hash128? version = null, uint crc32 = 0)
        {
            SetAsPromise();
            this.crc32 = crc32;
            this.version = version;
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
            wwwFuture = new WWWFuture(url + assetBundleName, Int32.MaxValue, version, crc32);
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
