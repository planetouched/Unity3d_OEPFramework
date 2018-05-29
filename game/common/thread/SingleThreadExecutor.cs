using System;
using System.Collections.Generic;
using System.Threading;
using Assets.OEPFramework.future;

namespace Assets.game.common.thread
{
    public class SingleThreadExecutor : IExecutor
    {
        readonly Queue<IFuture> tasks = new Queue<IFuture>();
        readonly ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        private readonly object syncRoot = new object();
        private volatile bool shutdown;
        private readonly Thread thread;
        public int taskCount { get { return _taskCount; } }
        int _taskCount;

        public SingleThreadExecutor(ThreadPriority threadPriority = ThreadPriority.Normal)
        {
            thread = new Thread(Work) { Priority = threadPriority, IsBackground = true };
            thread.Start();
            while (!thread.IsAlive) {}
        }

        void Work()
        {
            while (!shutdown)
            {
                lock (syncRoot)
                {
                    if (tasks.Count == 0)
                        manualResetEvent.Reset();
                }

                manualResetEvent.WaitOne();

                while (tasks.Count > 0)
                {
                    IFuture future;

                    lock (syncRoot)
                        future = tasks.Dequeue();

                    future.Run();
                    Interlocked.Decrement(ref _taskCount);
                }
            }
        }

        public void Shutdown()
        {
            shutdown = true;

            lock (syncRoot)
            {
                manualResetEvent.Set();
            }
        }

        public T Execute<T>(T future) where T : IFuture
        {
            if (shutdown)
                throw new Exception("executor was shutdown");

            Interlocked.Increment(ref _taskCount);
            lock (syncRoot)
            {
                tasks.Enqueue(future);
                manualResetEvent.Set();
            }
            return future;
        }

        public IFuture Execute(Action action)
        {
            if (shutdown)
                throw new Exception("executor Shutdown");

            Interlocked.Increment(ref _taskCount);
            IFuture future = new FutureTask(action);
            lock (syncRoot)
            {
                tasks.Enqueue(future);
                manualResetEvent.Set();
            }
            return future;
        }

        public IFuture Execute<T>(Func<T> func)
        {
            if (shutdown)
                throw new Exception("executor Shutdown");

            Interlocked.Increment(ref _taskCount);
            IFuture future = new FutureTask<T>(func);
            lock (syncRoot)
            {
                tasks.Enqueue(future);
                manualResetEvent.Set();
            }

            return future;
        }

        public void Join()
        {
            thread.Join();
        }
    }
}
