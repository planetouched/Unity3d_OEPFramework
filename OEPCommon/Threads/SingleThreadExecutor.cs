using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Basement.OEPFramework.Futures;

namespace OEPCommon.Threads
{
    public class SingleThreadExecutor : IExecutor
    {
        public int taskCount => _taskCount;
        
        private readonly ConcurrentQueue<IFuture> _tasks = new ConcurrentQueue<IFuture>();
        private readonly ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
        private volatile bool _shutdown;
        private readonly Thread _thread;
        private volatile int _taskCount;

        public SingleThreadExecutor(ThreadPriority threadPriority = ThreadPriority.Normal)
        {
            _thread = new Thread(Work) { Priority = threadPriority, IsBackground = true };
            _thread.Start();
            
            var spinWait = new SpinWait();
            
            while (!_thread.IsAlive)
            {
                spinWait.SpinOnce();
            }
        }

        private void Work()
        {
            var spinWait = new SpinWait();
            
            while (!_shutdown)
            {
                _manualResetEvent.WaitOne();
                _manualResetEvent.Reset();
                
                while (!_tasks.IsEmpty)
                {
                    IFuture future;

                    for (;;)
                    {
                        if (_tasks.TryDequeue(out future))
                        {
                            break;
                        }
                        
                        spinWait.SpinOnce();
                    }

                    future.Run();
                    Interlocked.Decrement(ref _taskCount);
                }
            }
        }

        public void Shutdown()
        {
            _shutdown = true;
            _manualResetEvent.Set();
        }

        public T Execute<T>(T future) where T : IFuture
        {
            if (_shutdown)
            {
                throw new Exception("executor is shutdown");
            }

            Interlocked.Increment(ref _taskCount);
            _tasks.Enqueue(future);
            _manualResetEvent.Set();
            
            return future;
        }

        public IFuture Execute(Action action)
        {
            if (_shutdown)
            {
                throw new Exception("executor is shutdown");
            }

            var future = new FutureTask(action);
            Interlocked.Increment(ref _taskCount);
            _tasks.Enqueue(future);
            _manualResetEvent.Set();
            
            return future;
        }

        public IFuture Execute<T>(Func<T> func)
        {
            if (_shutdown)
            {
                throw new Exception("executor is shutdown");
            }

            var future = new FutureTask<T>(func);
            Interlocked.Increment(ref _taskCount);
            _tasks.Enqueue(future);
            _manualResetEvent.Set();

            return future;
        }

        public void Join()
        {
            _thread.Join();
        }
    }
}
