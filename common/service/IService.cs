using OEPFramework.common.service.future;

namespace OEPFramework.common.service
{
    public interface IService
    {
        ManualStateFuture Start();
        ManualStateFuture Stop();
    }
}
