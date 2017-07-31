using OEPFramework.common.future;
using OEPFramework.common.service;
using OEPFramework.unityEngine.utils;
using UnityEngine;

namespace OEPFramework.unityEngine.assetBundle
{
    public class AssetBundleUnloadService : ServiceBase
    {
        public static string TRY_UNLOAD = GEvent.GetUniqueCategory();

        private readonly AssetBundleManager assetBundleManager;
        private Timer timer;
        private AsyncOperation lastAsyncOperation;
        private int packedSize;

        public override IFuture Start()
        {
            CheckState();
            GEvent.Attach(TRY_UNLOAD, OnTryUnload, null);
            return startFuture = FutureUtils.GetEmptyCompletedFuture();
        }

        public AssetBundleUnloadService(AssetBundleManager manager, int unloadPackedSizeInBytes)
        {
            assetBundleManager = manager;
            packedSize = unloadPackedSizeInBytes;
        }

        public void SetUnloadPacketSize(int sizeInBytes)
        {
            packedSize = sizeInBytes;
        }

        public override IFuture Stop()
        {
            CheckState();
            StopShedule();
            GEvent.Detach(TRY_UNLOAD, OnTryUnload);
            return stopFuture = FutureUtils.GetEmptyCompletedFuture();
        }

        public override IFuture Destroy()
        {
            CheckState();
            StopShedule();
            GEvent.Detach(TRY_UNLOAD, OnTryUnload);
            return destroyFuture = FutureUtils.GetEmptyCompletedFuture();
        }

        public void StartSchedule(float period)
        {
            StopShedule();
            timer = Timer.Create(period, OnTimer, null);
        }

        public void StopShedule()
        {
            if (timer != null)
                timer.Drop();
            timer = null;
        }

        private void OnTryUnload(object o)
        {
            TryUnload();
        }

        private void OnTimer()
        {
            TryUnload();
        }

        public void TryUnload()
        {
            if (assetBundleManager.GetLoadedPackedSize() >= packedSize)
                ForceTryUnload();
        }

        public AsyncOperation ForceTryUnload()
        {
            if (lastAsyncOperation != null && !lastAsyncOperation.isDone)
                return lastAsyncOperation;

            lastAsyncOperation = assetBundleManager.UnloadUnused();
            return lastAsyncOperation;
        }
    }
}
