using OEPFramework.common.byteBuf;

namespace OEPFramework.service.socketService.message
{
    public abstract class ReceivableMessageBase : MessageBase
    {
        protected ByteBufReader bufReader;
        
        public abstract void Decode(byte [] bytes);
    }
}
