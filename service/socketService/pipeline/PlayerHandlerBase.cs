using System.Collections.Generic;
using OEPFramework.common;
using OEPFramework.common.pipeline;
using OEPFramework.service.socketService.message;

namespace OEPFramework.service.socketService.pipeline
{
    public abstract class PlayerHandlerBase : PipelineHandlerBase
    {
        public override void Create(object data)
        {
            var messages = (List<Tuple<ReceivableMessageBase, IReceiveHandler>>) data;
            foreach (var tuple in messages)
            {
                Process(tuple.value1, tuple.value2);
            }
        }
        public abstract void Process(ReceivableMessageBase message, IReceiveHandler handler);
    }
}
