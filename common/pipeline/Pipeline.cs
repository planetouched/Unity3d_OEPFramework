using System;
using System.Collections.Generic;

namespace OEPFramework.common.pipeline
{
    public class Pipeline
    {
        internal class PipelineOption
        {
            private readonly Func<IPipelineHandler> factory;
            private readonly IPipelineHandler handler;

            public PipelineOption(Func<IPipelineHandler> factory, IPipelineHandler handler)
            {
                this.factory = factory;
                this.handler = handler;
            }

            public IPipelineHandler GetHandler()
            {
                if (factory != null)
                    return factory();

                return handler;
            }
        }

        private readonly List<PipelineOption> pipelineHandlers = new List<PipelineOption>();

        public event Action<IPipelineHandler> onError;
        public event Action<IPipelineHandler> onHandlerPassed;

        public Pipeline AddHandler(Func<IPipelineHandler> handlerFactory)
        {
            pipelineHandlers.Add(new PipelineOption(handlerFactory, null));
            return this;
        }

        public Pipeline AddHandler(IPipelineHandler handler)
        {
            pipelineHandlers.Add(new PipelineOption(null, handler));
            return this;
        }

        public Pipeline RemoveHandler(int idx)
        {
            pipelineHandlers.RemoveAt(idx);
            return this;
        }

        public object DoPipeline(object item)
        {
            object currentItem = item;
            foreach (var factory in pipelineHandlers)
            {
                var handler = factory.GetHandler();
                handler.Create(currentItem);

                var newElement = handler.ReturnItem();
                if (newElement != null)
                    currentItem = newElement;

                if (onHandlerPassed != null)
                    onHandlerPassed(handler);

                if (handler.GetError() != 0)
                {
                    if (onError != null)
                        onError(handler);
                    return null;
                }
            }
            return currentItem;
        }
    }
}
