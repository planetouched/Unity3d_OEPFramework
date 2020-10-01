using System;
using Basement.OEPFramework.Futures;
using UnityEngine;

namespace OEPCommon.AssetBundles.Futures
{
    public class AssetBundleCreateRequestPromise : Future
    {
        private readonly Func<AssetBundleCreateRequest> _asyncOperationFunc;
        public AssetBundleCreateRequest result { get; private set; }

        public AssetBundleCreateRequestPromise(Func<AssetBundleCreateRequest> asyncOperationFunc)
        {
            SetAsPromise();
            _asyncOperationFunc = asyncOperationFunc;
        }
        
        protected override void OnRun()
        {
            result = _asyncOperationFunc();
            
            if (result.isDone)
            {
                Complete();
                return;
            }
            
            result.completed += operation =>
            {
                if (result.isDone)
                {
                    Complete();
                }
                else
                {
                    throw new Exception("Async operation wasn't completed properly");                
                }
            }; 
        }

        protected override void OnComplete()
        {
        }
    }
}