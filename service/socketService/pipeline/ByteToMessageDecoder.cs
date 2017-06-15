using System;
using System.Collections.Generic;
using OEPFramework.common.pipeline;
using OEPFramework.service.socketService.message;

namespace OEPFramework.service.socketService.pipeline
{
    public class ByteToMessageDecoder : PipelineHandlerBase
    {
        private readonly List<byte[]> byteMessagesList = new List<byte[]>();
        private readonly List<byte> storedBytes = new List<byte>();
        private readonly int maxMessageSize;

        public ByteToMessageDecoder(int maxMessageSize)
        {
            this.maxMessageSize = maxMessageSize;
        }

        public override void Create(object data)
        {
            var bytes = (byte[])data;
            storedBytes.AddRange(bytes);

            while (storedBytes.Count >= MessageBase.MessageHeaderSize)
            {
                int messageSize = (bytes[2] << 8) + bytes[3];

                if (messageSize >= maxMessageSize)
                {
                    storedBytes.Clear();
                    byteMessagesList.Clear();
                    SetError(2);
                    throw new Exception("messageSize >= maxMessageSize");
                }

                if (storedBytes.Count >= messageSize)
                {
                    var fullMessage = new byte[messageSize];
                    storedBytes.CopyTo(0, fullMessage, 0, messageSize);
                    storedBytes.RemoveRange(0, messageSize);

                    byteMessagesList.Add(fullMessage);
                }
                else
                    break;
            }

            if (byteMessagesList.Count == 0)
                SetError(1);
        }

        public override object ReturnItem()
        {
            var clone = new List<byte[]>(byteMessagesList);
            byteMessagesList.Clear();
            return clone;
        }
    }
}
