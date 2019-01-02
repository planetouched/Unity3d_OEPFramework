using System;
using System.Collections.Generic;

namespace OEPFramework.futures.utils
{
    public class FutureQueue
    {
        readonly List<IFuture> queueFutures = new List<IFuture>();
        private IFuture current;
        public int futuresCount { get { return queueFutures.Count; } }
        public event Action<IFuture> onFutureComplete;

        public IFuture Add(IFuture future)
        {
            if (future.isDone || future.isCancelled || future.wasRun)
                throw new Exception("future already run or completed");

            queueFutures.Add(future);
            future.AddListener(FutureComplete);
            if (queueFutures.Count == 1)
            {
                current = future;
                future.Run();
            }

            return future;
        }

        private void FutureComplete(IFuture f)
        {
            if (onFutureComplete != null)
                onFutureComplete(f);

            queueFutures.Remove(f);
            if (queueFutures.Count > 0)
            {
                if (current == f)
                    current = queueFutures[0];
            }
            else
                current = null;

            if (current != null)
                current.Run();
        }

        public void CancelCurrent()
        {
            if (current != null)
                current.Cancel();
        }

        public void Cancel()
        {
            var copy = new List<IFuture>(queueFutures);
            queueFutures.Clear();

            foreach (var future in copy)
            {
                future.RemoveListener(FutureComplete);
                future.Cancel();
            }

            onFutureComplete = null;
        }
    }
}
