using Assets.OEPFramework.unityEngine;
using UnityEngine;

namespace Assets.game.assetBundle
{
    public class AssetBundleUnloadService
    {
        public static string TRY_UNLOAD = GEvent.GetUniqueCategory();

        private readonly AssetBundleManager assetBundleManager;
        private Timer timer;
        private AsyncOperation lastAsyncOperation;
        private int packedSize;
        private readonly float cleanPeriod;

        public AssetBundleUnloadService(AssetBundleManager manager, int unloadPackedSizeInBytes, float cleanPeriod = -1)
        {
            this.cleanPeriod = cleanPeriod;
            assetBundleManager = manager;
            packedSize = unloadPackedSizeInBytes;
        }

        public void SetUnloadPacketSize(int sizeInBytes)
        {
            packedSize = sizeInBytes;
        }


        public void Start()
        {
            if (cleanPeriod > 0)
                StartSchedule(cleanPeriod);

            GEvent.Attach(TRY_UNLOAD, OnTryUnload, null);
        }

        public void Stop()
        {
            StopShedule();
            GEvent.Detach(TRY_UNLOAD, OnTryUnload);
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
            if (assetBundleManager.loadedPackedSize >= packedSize)
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
