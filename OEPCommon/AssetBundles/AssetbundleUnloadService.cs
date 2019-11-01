using Basement.OEPFramework.UnityEngine;
using UnityEngine;

namespace OEPCommon.AssetBundles
{
    public static class AssetBundleUnloadService
    {
        private static Timer _timer;
        private static AsyncOperation _lastAsyncOperation;
        private static int _packedSize;

        public static void Init(int unloadPackedSizeInBytes)
        {
            _packedSize = unloadPackedSizeInBytes;
        }

        public static void SetUnloadPackedSize(int sizeInBytes)
        {
            _packedSize = sizeInBytes;
        }

        public static void StartSchedule(float period)
        {
            StopSchedule();
            _timer = Timer.Create(period, OnTimer, null);
        }

        public static void StopSchedule()
        {
            _timer?.Drop();
            _timer = null;
        }

        public static AsyncOperation TryUnload()
        {
            if (AssetBundleManager.loadedPackedSize >= _packedSize)
            {
                return ForceTryUnload();
            }

            return null;
        }

        public static AsyncOperation ForceTryUnload()
        {
            if (_lastAsyncOperation != null && !_lastAsyncOperation.isDone)
            {
                return _lastAsyncOperation;
            }

            _lastAsyncOperation = AssetBundleManager.UnloadUnused();
            return _lastAsyncOperation;
        }
        
        private static void OnTimer()
        {
            TryUnload();
        }
    }
}
