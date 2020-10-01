using System;
using System.Threading.Tasks;
using Basement.OEPFramework.Futures;
using Basement.OEPFramework.UnityEngine;

namespace OEPCommon.AssetBundles.Futures
{
    public class UnsafeTask2Future<T> : ThreadSafeFuture
    {
        public T result { get; private set; } 
        
        private readonly Func<Task<T>> _taskFunc;
        private readonly int _syncLoop;
        
        public UnsafeTask2Future(Func<Task<T>> taskFunc, int syncLoop = -1)
        {
            _taskFunc = taskFunc;
            _syncLoop = syncLoop;
        }
        
        protected override void OnRun()
        {
            _taskFunc().ContinueWith(TaskEnd, TaskContinuationOptions.ExecuteSynchronously);
        }

        private void TaskEnd(Task<T> task)
        {
            if (task.IsCompleted)
            {
                result = task.Result;
                
                if (_syncLoop != -1)
                {
                    Sync.Add(Complete, _syncLoop);
                }
                else
                {
                    Complete();
                }
                return;
            }

            if (task.IsCanceled || task.IsFaulted)
            {
                if (_syncLoop != -1)
                {
                    Sync.Add(Cancel, _syncLoop);
                }
                else
                {
                    Cancel();
                }
            }
        }

        protected override void OnComplete()
        {
        }
    }
}