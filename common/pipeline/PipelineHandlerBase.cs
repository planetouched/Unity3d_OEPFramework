using System.Threading;

namespace Assets.common.pipeline
{
    public abstract class PipelineHandlerBase : IPipelineHandler
    {
        private int error;
        private readonly ManualResetEvent resetEvent = new ManualResetEvent(true);

        public abstract object ReturnItem();
        public abstract void Create(object data);

        public int GetError()
        {
            return error;
        }

        public void SetError(int errorCode)
        {
            error = errorCode;
        }

        public void Sleep()
        {
            resetEvent.Reset();
            resetEvent.WaitOne();
        }

        public void Wakeup()
        {
            resetEvent.Set();
        }
    }
}
