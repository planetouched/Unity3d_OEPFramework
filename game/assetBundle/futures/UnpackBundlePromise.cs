using OEPFramework.unityEngine.behaviour;
using OEPFramework.unityEngine.loop;
using UnityEngine;

namespace game.assetBundle.futures
{
    public class UnpackBundlePromise : FutureBehaviour
    {
        private readonly AssetBundle assetBundle;
        public AssetBundleRequest asyncOperation { get; private set; } 
        public Object[] allAssets { get; private set; }
        private readonly bool async;

        public UnpackBundlePromise(AssetBundle assetBundle, bool async)
        {
            SetAsPromise();
            this.async = async;
            this.assetBundle = assetBundle;
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
            if (async)
                asyncOperation = assetBundle.LoadAllAssetsAsync();
            else
            {
                allAssets = assetBundle.LoadAllAssets();
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
