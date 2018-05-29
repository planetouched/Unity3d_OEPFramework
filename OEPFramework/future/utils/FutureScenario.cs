using System;
using System.Collections.Generic;

namespace Assets.OEPFramework.future.utils
{
    public class FutureScenario
    {
        readonly List<CompositeFuture> compositeFutures = new List<CompositeFuture>();
        private CompositeFuture current;
        public event Action<bool> onComplete;
        public bool isRun { get; private set; }
        public bool isCancelled { get; private set; }
        public bool isEmpty { get { return compositeFutures.Count == 1 && compositeFutures[0].futuresCount == 0; } }

        public FutureScenario()
        {
            Init();
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
            isRun = false;
            Init();

            if (onComplete != null)
                onComplete(isCancelled);
            onComplete = null;
        }
        
        void Init()
        {
            compositeFutures.Add(new CompositeFuture());
            current = compositeFutures[0];
            current.AddListener(CompleteFuture);
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
            if (isRun || compositeFutures[0].futuresCount == 0) return;
            isRun = true;
            isCancelled = false;
            compositeFutures[0].Run();
        }

        public void AddFuture(IFuture future)
        {
            if (future.wasRun || future.isCancelled || future.isDone)
                throw new Exception("future already run or completed");

            current.AddFuture(future);
        }

        public void ExecuteTask(Action method)
        {
            AddFuture(new FutureTask(method));
        }

        public void Cancel()
        {
            if (isCancelled) return;
            isCancelled = true;
            var cpy = new List<CompositeFuture>(compositeFutures);
            compositeFutures.Clear();

            foreach (var f in cpy)
            {
                f.RemoveListener(CompleteFuture);
                f.Cancel();
            }

            Complete();
        }
    }
}
