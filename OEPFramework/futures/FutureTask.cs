using System;

namespace OEPFramework.futures
{
    public class FutureTask<T> : ThreadSafeFuture
    {
        public T result { get; private set; }
        private Func<T> func;
        public FutureTask(Func<T> func)
        {
            this.func = func;
        }

        protected override void OnRun()
        {
            result = func();
            Complete();
        }

        protected override void OnComplete()
        {
            func = null;
        }
    }
    
    public class FutureTask : ThreadSafeFuture
    {
        private Action action;
        public FutureTask(Action action)
        {
            this.action = action;
        }

        protected override void OnRun()
        {
            action();
            Complete();
        }

        protected override void OnComplete()
        {
            action = null;
        }
    }
}