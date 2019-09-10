using Basement.OEPFramework.UnityEngine;
using UnityEngine;

namespace Game.AssetBundle
{
    public class AssetBundleUnloadService
    {
        public static string TRY_UNLOAD = GEvent.GetUniqueCategory();

        private readonly AssetBundleManager _assetBundleManager;
        private Timer _timer;
        private AsyncOperation _lastAsyncOperation;
        private int _packedSize;
        private readonly float _cleanPeriod;

        public AssetBundleUnloadService(AssetBundleManager manager, int unloadPackedSizeInBytes, float cleanPeriod = -1)
        {
            _cleanPeriod = cleanPeriod;
            _assetBundleManager = manager;
            _packedSize = unloadPackedSizeInBytes;
        }

        public void SetUnloadPacketSize(int sizeInBytes)
        {
            _packedSize = sizeInBytes;
        }

        public void Start()
        {
            if (_cleanPeriod > 0)
                StartSchedule(_cleanPeriod);

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
            _timer = Timer.Create(period, OnTimer, null);
        }

        public void StopShedule()
        {
            _timer?.Drop();
            _timer = null;
        }

        public void TryUnload()
        {
            if (_assetBundleManager.loadedPackedSize >= _packedSize)
            {
                ForceTryUnload();
            }
        }

        public AsyncOperation ForceTryUnload()
        {
            if (_lastAsyncOperation != null && !_lastAsyncOperation.isDone)
            {
                return _lastAsyncOperation;
            }

            _lastAsyncOperation = _assetBundleManager.UnloadUnused();
            return _lastAsyncOperation;
        }
        
        private void OnTryUnload(object o)
        {
            TryUnload();
        }

        private void OnTimer()
        {
            TryUnload();
        }
    }
}
