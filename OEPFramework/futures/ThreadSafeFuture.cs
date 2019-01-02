using System;
using System.Threading;

namespace Assets.OEPFramework.futures
{
    public abstract class ThreadSafeFuture : IFuture
    {
        static int globalHashCode;
        private readonly int hashCode;

        public object syncRoot { get; private set; }
        public bool isCancelled { get; private set; }
        public bool isDone { get; private set; }
        public bool wasRun { get; private set; }

        event Action<IFuture> onComplete;
        event Action<IFuture> onRun;
        private bool promise;

        protected ThreadSafeFuture ()
        {
            syncRoot = new object();
            hashCode = Interlocked.Increment(ref globalHashCode);
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        void CallRunHandlers()
        {
            if (onRun != null)
                onRun(this);
            onRun = null;
        }

        void CallHandlers()
        {
            if (onComplete != null)
                onComplete(this);
            onComplete = null;
        }

        public IFuture AddListenerOnRun(Action<IFuture> method)
        {
            bool call = false;
            lock (syncRoot)
            {
                if (!wasRun)
                    onRun += method;
                else
                    call = true;
            }

            if (call)
                method(this);

            return this;
        }

        public void RemoveListenerOnRun(Action<IFuture> method)
        {
            lock (syncRoot)
                onRun -= method;
        }

        public IFuture AddListener(Action<IFuture> method)
        {
            bool call = false;
            lock (syncRoot)
            {
                if (!isDone && !isCancelled)
                    onComplete += method;
                else
                    call = true;
            }

            if (call)
                method(this);

            return this;
        }


        public void RemoveListener(Action<IFuture> method)
        {
            lock (method)
                onComplete -= method;
        }

        public void Cancel()
        {
            lock (syncRoot)
            {
                if (promise || isCancelled || isDone)
                    return;
                isCancelled = true;
            }

            OnComplete();
            CallHandlers();
        }

        public void Complete()
        {
            lock (syncRoot)
            {
                if (isCancelled || isDone)
                    return;
                isDone = true;
            }

            OnComplete();
            CallHandlers();
        }

        public IFuture Run()
        {
            lock (syncRoot)
            {
                if (wasRun || isCancelled || isDone) return this;
                wasRun = true;
            }

            OnRun();
            CallRunHandlers();
            return this;
        }

        protected abstract void OnRun();
        protected abstract void OnComplete();

        public static T StaticCast<T>(IFuture future)
        {
            return (T)future;
        }

        public T Cast<T>() where T : IFuture
        {
            return StaticCast<T>(this);
        }

        public IFuture SetAsPromise()
        {
            promise = true;
            return this;
        }
    }
}
