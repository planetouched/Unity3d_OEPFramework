using Basement.OEPFramework.Futures;
using UnityEngine;

namespace Game.AssetBundle.Futures
{
    public class UnpackBundlePromise : Future
    {
        public AssetBundleRequest asyncOperation { get; private set; } 
        public Object[] allAssets { get; private set; }
        
        private readonly UnityEngine.AssetBundle _assetBundle;
        private readonly bool _async;

        public UnpackBundlePromise(UnityEngine.AssetBundle assetBundle, bool async)
        {
            SetAsPromise();
            _async = async;
            _assetBundle = assetBundle;
        }

        protected override void OnRun()
        {
            if (_async)
            {
                asyncOperation = _assetBundle.LoadAllAssetsAsync();
            }
            else
            {
                allAssets = _assetBundle.LoadAllAssets();
                Complete();
                return;
            }

            asyncOperation.completed += _ =>
            {
                allAssets = asyncOperation.allAssets;
                Complete();
            };
        }

        protected override void OnComplete()
        {
        }
    }
}
