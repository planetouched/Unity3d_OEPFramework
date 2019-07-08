using OEPFramework.unityEngine.behaviour;
using OEPFramework.unityEngine.loop;
using UnityEngine;

namespace game.assetBundle.futures
{
    public class UnpackBundlePromise : FutureBehaviour
    {
        public AssetBundleRequest asyncOperation { get; private set; } 
        public Object[] allAssets { get; private set; }
        
        private readonly AssetBundle _assetBundle;
        private readonly bool _async;

        public UnpackBundlePromise(AssetBundle assetBundle, bool async)
        {
            SetAsPromise();
            _async = async;
            _assetBundle = assetBundle;
            LoopOn(Loops.UPDATE, Update);
        }

        private void Update()
        {
            if (asyncOperation.isDone)
            {
                allAssets = asyncOperation.allAssets;
                Complete();
            }
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

            Play();
        }

        protected override void OnComplete()
        {
        }
    }
}
