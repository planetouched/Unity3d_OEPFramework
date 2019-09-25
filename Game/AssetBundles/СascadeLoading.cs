using System;
using System.Collections.Generic;
using Basement.OEPFramework.Futures;
using Basement.OEPFramework.Futures.Util;

namespace Game.AssetBundle
{
    public class CascadeLoading
    {
        public event Action onComplete;
        private readonly List<CompositeFuture> _compositeFutures = new List<CompositeFuture>();
        private CompositeFuture _current;
        
        public CascadeLoading()
        {
            _compositeFutures.Add(new CompositeFuture());
            _current = _compositeFutures[0];
            _current.AddListener(CompleteFuture);
        }

        public void ClearEvent()
        {
            onComplete = null;
        }
        
        private void CompleteFuture(IFuture future)
        {
            IFuture nextFuture = null;
            
            _compositeFutures.RemoveAt(0);

            if (_compositeFutures.Count > 0)
            {
                nextFuture = _compositeFutures[0];
            }

            if (nextFuture == null)
            {
                Complete();
            }
            else
            {
                nextFuture.Run();
            }
        }
        
        private void Complete()
        {
            _current = null;
            onComplete?.Invoke();
            onComplete = null;
        }
        
        public void Next()
        {
            if (_current.futuresCount == 0)
            {
                return;
            }

            var newFuture = new CompositeFuture();
            _compositeFutures.Add(newFuture);
            newFuture.AddListener(CompleteFuture);
            _current = newFuture;
        }
        
        public void Run()
        {
            _compositeFutures[0].Run();
        }
        
        public void AddFuture(IFuture future)
        {
            _current.AddFuture(future);
        }
    }
}