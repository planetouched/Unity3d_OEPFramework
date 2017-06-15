using System;
using OEPFramework.common.service.future;

namespace OEPFramework.common.service
{
    public abstract class ServiceBase : IService
    {
        protected ManualStateFuture startFuture;
        protected ManualStateFuture stopFuture;
        protected ManualStateFuture destroyFuture;

        protected void CheckState()
        {
            if (startFuture != null && startFuture.wasRun)
                throw new Exception("Service starts...");
            if (stopFuture != null && stopFuture.wasRun)
                throw new Exception("Service stops...");
            if (destroyFuture != null && destroyFuture.wasRun)
                throw new Exception("Service destroyed...");
        }

        public abstract ManualStateFuture Start();
        public abstract ManualStateFuture Stop();
        public abstract ManualStateFuture Destroy();

        protected ManualStateFuture GetEmptyCompletedFuture()
        {
            var f = new ManualStateFuture();
            f.Complete();
            return f;
        }

    }
}
