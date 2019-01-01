using System;
using System.Collections.Generic;
using OEPFramework.future;
using OEPFramework.future.utils;

namespace Assets.game.assetBundle
{
    public class CascadeLoading
    {
        readonly List<CompositeFuture> compositeFutures = new List<CompositeFuture>();
        private CompositeFuture current;
        public Action onComplete;
        public CascadeLoading()
        {
            compositeFutures.Add(new CompositeFuture());
            current = compositeFutures[0];
            current.AddListener(CompleteFuture);
        }
        void CompleteFuture(IFuture future)
        {
            IFuture nextFuture = null;
            compositeFutures.RemoveAt(0);
            if (compositeFutures.Count > 0)
                nextFuture = compositeFutures[0];
            if (nextFuture == null)
                Complete();
            else
                nextFuture.Run();
        }
        void Complete()
        {
            current = null;
            if (onComplete != null)
                onComplete();
            onComplete = null;
        }
        public void Next()
        {
            if (current.futuresCount == 0) return;
            var newFuture = new CompositeFuture();
            compositeFutures.Add(newFuture);
            newFuture.AddListener(CompleteFuture);
            current = newFuture;
        }
        public void Run()
        {
            compositeFutures[0].Run();
        }
        public void AddFuture(IFuture future)
        {
            current.AddFuture(future);
        }
    }
}