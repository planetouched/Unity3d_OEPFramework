using System.Collections.Generic;

namespace OEPFramework.futures.utils
{
    public class CompositeFuture : FutureBase
    {
        public int futuresCount => _futures.Count;

        private readonly List<IFuture> _futures = new List<IFuture>();

        public List<IFuture> GetFuturesCopy()
        {
            return new List<IFuture>(_futures);
        }

        public override void Cancel()
        {
            if (promise || isCancelled || isDone)
                return;
            isCancelled = true;
            wasRun = false;
            var copy = GetFuturesCopy();
            _futures.Clear();

            foreach (var future in copy)
            {
                if (future.isCancelled) continue;
                future.RemoveListener(OnFutureComplete);
                future.Cancel();
            }

            CallHandlers();
        }

        public void AddFuture(IFuture future)
        {
            if (wasRun || isDone || isCancelled || future.isDone || future.isCancelled)
                return;

            _futures.Add(future);
            future.AddListener(OnFutureComplete);
        }

        private void OnFutureComplete(IFuture future)
        {
            _futures.Remove(future);
            future.RemoveListener(OnFutureComplete);

            if (_futures.Count > 0) return;
            isDone = true;
            wasRun = false;

            CallHandlers();
        }

        public override IFuture Run()
        {
            if (wasRun) return this;
            wasRun = true;
            var copyList = GetFuturesCopy();
            isDone = copyList.Count == 0;

            CallRunHandlers();

            if (isDone)
            {
                wasRun = false;
                CallHandlers();
            }
            else
            {
                foreach (var future in copyList)
                    future.Run();
            }
            return this;
        }
    }
}
