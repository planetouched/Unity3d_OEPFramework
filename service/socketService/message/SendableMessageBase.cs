using System.Collections.Generic;
using OEPFramework.common.byteBuf;

namespace OEPFramework.service.socketService.message
{
    public abstract class SendableMessageBase : MessageBase
    {
        protected ByteBufWriter bufWriter;
        public abstract int GetId();

        protected void CreateHeader()
        {
            bufWriter = new ByteBufWriter(new List<byte>());
            bufWriter.WriteUShort((ushort)GetId());
            bufWriter.WriteUShort(0); //size reserve
        }

        public void Send(SocketClient client)
        {
            bufWriter.SetOffset(2);
            bufWriter.WriteUShort((ushort)(bufWriter.Length()));
            var bytes = bufWriter.GetBuffer();
            client.Send(bytes);
        }
    }
}
