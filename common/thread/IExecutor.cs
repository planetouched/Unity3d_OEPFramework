using System;
using OEPFramework.common.future;

namespace OEPFramework.common.thread
{
    public interface IExecutor
    {
        int taskCount { get; }
        void Shutdown();
        IFuture Execute(Action action);
        IFuture Execute<T>(Func<T> func);
        T Execute<T>(T future) where T : IFuture;
        void Join();
    }
}
