using OEPFramework.service.socketService.message;

namespace OEPFramework.service.socketService
{
    public interface IReceiveHandler
    {
        void OnReceive(ReceivableMessageBase message, SocketClient client);
    }
}
