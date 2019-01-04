using System;
using System.Collections.Generic;
using OEPFramework.futures;
using OEPFramework.futures.utils;

namespace game.assetBundle
{
    public class CascadeLoading
    {
        public Action onComplete;
        private readonly List<CompositeFuture> _compositeFutures = new List<CompositeFuture>();
        private CompositeFuture _current;
        
        public CascadeLoading()
        {
            _compositeFutures.Add(new CompositeFuture());
            _current = _compositeFutures[0];
            _current.AddListener(CompleteFuture);
        }
        
        void CompleteFuture(IFuture future)
        {
            IFuture nextFuture = null;
            _compositeFutures.RemoveAt(0);
            if (_compositeFutures.Count > 0)
                nextFuture = _compositeFutures[0];
            if (nextFuture == null)
                Complete();
            else
                nextFuture.Run();
        }
        
        void Complete()
        {
            _current = null;
            if (onComplete != null)
                onComplete();
            onComplete = null;
        }
        
        public void Next()
        {
            if (_current.futuresCount == 0) return;
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