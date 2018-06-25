using Assets.OEPFramework.future;

namespace Assets.common.service
{
    public enum ServiceState
    {
        Started, Stops, Stopped, Working
    }

    public interface IService
    {
        ServiceState state { get; }

        IFuture Start();
        IFuture Stop();
    }
}
