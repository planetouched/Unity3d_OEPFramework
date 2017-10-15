using System;
using OEPFramework.common.service.future;

namespace OEPFramework.common.service
{
    public enum ServiceState
    {
        Start, Stop
    }

    public abstract class ServiceBase : IService
    {
        public ServiceState state { get; protected set; }
        private ManualStateFuture currentFuture;

        protected ServiceBase()
        {
            state = ServiceState.Stop;
        }

        void Check()
        {
            if (currentFuture != null)
                throw new Exception("Service locked");
        }

        public virtual ManualStateFuture Start()
        {
            if (state == ServiceState.Start)
                throw new Exception("Service already started");

            Check();

            currentFuture = new ManualStateFuture();
            currentFuture.AddListener(f =>
            {
                if (f.isDone)
                {
                    state = ServiceState.Start;
                }
                currentFuture = null;
            });

            return currentFuture;
        }

        public virtual ManualStateFuture Stop()
        {
            if (state == ServiceState.Stop)
                throw new Exception("Service already stopped");

            Check();

            state = ServiceState.Stop;
            currentFuture = new ManualStateFuture();
            currentFuture.AddListener(f =>
            {
                currentFuture = null;
            });

            return currentFuture;
        }
    }
}
