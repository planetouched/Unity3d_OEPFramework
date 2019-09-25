using Basement.OEPFramework.Futures;
using UnityEngine;

namespace Game.AssetBundles.Futures
{
    public class UnpackBundlePromise : Future
    {
        public float asyncOperationProgress => _asyncOperation?.progress ?? 1;  
        public Object[] allAssets { get; private set; }
        
        private readonly AssetBundle _assetBundle;
        private readonly bool _async;
        private AssetBundleRequest _asyncOperation;

        public UnpackBundlePromise(AssetBundle assetBundle, bool async)
        {
            SetAsPromise();
            _async = async;
            _assetBundle = assetBundle;
        }

        protected override void OnRun()
        {
            if (_async)
            {
                _asyncOperation = _assetBundle.LoadAllAssetsAsync();
                
                _asyncOperation.completed += _ =>
                {
                    allAssets = _asyncOperation.allAssets;
                    Complete();
                };
            }
            else
            {
                allAssets = _assetBundle.LoadAllAssets();
                Complete();
            }
        }

        protected override void OnComplete()
        {
        }
    }
}
