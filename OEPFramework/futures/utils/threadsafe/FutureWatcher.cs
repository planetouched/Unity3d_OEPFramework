using System.Collections.Generic;

namespace OEPFramework.futures.utils.threadsafe
{
    public class FutureWatcher
    {
        private readonly object _locker = new object();
        //public List<IFuture> futures = new List<IFuture>();
        public HashSet<IFuture> futures = new HashSet<IFuture>();
        public int futuresCount { get { return futures.Count; } }

        public void AddFuture(IFuture future)
        {
            if (future == null) return;

            lock (_locker)
            {
                if (futures.Contains(future)) return;
                futures.Add(future);
            }

            future.AddListener(InnerRemoveFuture);
        }

        private void InnerRemoveFuture(IFuture future)
        {
            lock (_locker)
                futures.Remove(future);

            future.RemoveListener(InnerRemoveFuture);
        }

        public void CancelFutures()
        {
            IList<IFuture> copy;
            lock (_locker)
            {
                copy = new List<IFuture>(futures);
                futures.Clear();
            }

            foreach (var f in copy)
            {
                f.RemoveListener(InnerRemoveFuture);
                f.Cancel();
            }
        }
    }
}
