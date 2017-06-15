using System;
using System.Collections.Generic;
using OEPFramework.common;
using OEPFramework.service.socketService.message;

namespace OEPFramework.service.socketService
{
    public class MessageFactory
    {
        private readonly Dictionary<int, Tuple<Func<ReceivableMessageBase>, IReceiveHandler>> factories = new Dictionary<int, Tuple<Func<ReceivableMessageBase>, IReceiveHandler>>();

        public void Register(int id, Func<ReceivableMessageBase> func, IReceiveHandler handler)
        {
            factories.Add(id, new Tuple<Func<ReceivableMessageBase>, IReceiveHandler>(func, handler));
        }

        public Tuple<ReceivableMessageBase, IReceiveHandler> Build(byte [] bytes)
        {
            int messageId = (bytes[0] << 8) + bytes[1];

            Tuple<Func<ReceivableMessageBase>, IReceiveHandler> item;
            if (factories.TryGetValue(messageId, out item))
            {
                var pair = factories[messageId];
                var message = pair.value1();
                message.Decode(bytes);
                return new Tuple<ReceivableMessageBase, IReceiveHandler>(message, pair.value2);
            }

            return null;
        }
    }
}
