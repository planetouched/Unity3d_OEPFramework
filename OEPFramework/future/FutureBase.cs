﻿using System;
using System.Threading;

#if REFVIEW
using OEPFramework.utils;
#endif

namespace OEPFramework.future
{
    public abstract class FutureBase :
#if REFVIEW
    ReferenceCounter,
#endif
        IFuture
    {
        static int globalHashCode;
        private readonly int hashCode;

        public object syncRoot { get; private set; }
        public bool isCancelled { get; protected set; }
        public bool isDone { get; protected set; }
        public bool wasRun { get; protected set; }

        event Action<IFuture> onComplete;
        event Action<IFuture> onRun;
        protected bool promise;

        public override int GetHashCode()
        {
            return hashCode;
        }

        protected FutureBase()
        {
            syncRoot = new object();
            hashCode = Interlocked.Increment(ref globalHashCode);
        }

        protected void CallRunHandlers()
        {
            if (onRun != null)
                onRun(this);
            onRun = null;
        }

        protected void CallHandlers()
        {
            if (onComplete != null)
                onComplete(this);
            onComplete = null;
        }

        public IFuture AddListenerOnRun(Action<IFuture> method)
        {
            if (!wasRun)
                onRun += method;
            else
                method(this);

            return this;
        }

        public void RemoveListenerOnRun(Action<IFuture> method)
        {
            onRun -= method;
        }

        public IFuture AddListener(Action<IFuture> method)
        {
            if (!isDone && !isCancelled)
                onComplete += method;
            else
                method(this);

            return this;
        }


        public void RemoveListener(Action<IFuture> method)
        {
            onComplete -= method;
        }

        public abstract void Cancel();
        public abstract IFuture Run();

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
