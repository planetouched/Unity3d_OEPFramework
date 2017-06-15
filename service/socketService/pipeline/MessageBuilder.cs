using System;
using System.Collections.Generic;
using OEPFramework.common;
using OEPFramework.common.pipeline;
using OEPFramework.service.socketService.message;

namespace OEPFramework.service.socketService.pipeline
{
    public class MessageBuilder : PipelineHandlerBase
    {
        private readonly MessageFactory factory;
        private readonly List<Tuple<ReceivableMessageBase, IReceiveHandler>> messages = new List<Tuple<ReceivableMessageBase, IReceiveHandler>>();

        public MessageBuilder(MessageFactory messageFactory)
        {
            factory = messageFactory;
        }

        public override void Create(object data)
        {
            var byteMessagesList = (List<byte[]>)data;
            foreach (var byteMessage in byteMessagesList)
            {
                var tuple = factory.Build(byteMessage);

                if (tuple == null)
                    throw new Exception("Message not registered");

                messages.Add(tuple);
            }

            if (messages.Count == 0)
                SetError(1);
        }

        public override object ReturnItem()
        {
            var clone = new List<Tuple<ReceivableMessageBase, IReceiveHandler>>(messages);
            messages.Clear();
            return clone;
        }
    }
}
