using System;
using System.Collections.Generic;

namespace OEPCommon.Pipelines
{
    public class Pipeline
    {
        private class PipelineOption
        {
            private readonly Func<IPipelineHandler> _factory;
            private readonly IPipelineHandler _handler;

            public PipelineOption(Func<IPipelineHandler> factory, IPipelineHandler handler)
            {
                _factory = factory;
                _handler = handler;
            }

            public IPipelineHandler GetHandler()
            {
                return _factory != null ? _factory() : _handler;
            }

            public bool Compare(IPipelineHandler handler)
            {
                return _handler == handler;
            }
            
            public bool Compare(Func<IPipelineHandler> factory)
            {
                return _factory == factory;
            }
        }

        private readonly List<PipelineOption> _pipelineHandlers = new List<PipelineOption>();

        public event Action<IPipelineHandler> onError;
        public event Action<IPipelineHandler> onHandlerPassed;

        public Pipeline AddHandler(Func<IPipelineHandler> handlerFactory)
        {
            _pipelineHandlers.Add(new PipelineOption(handlerFactory, null));
            return this;
        }

        public Pipeline AddHandler(IPipelineHandler handler)
        {
            _pipelineHandlers.Add(new PipelineOption(null, handler));
            return this;
        }

        public Pipeline RemoveHandler(int idx)
        {
            _pipelineHandlers.RemoveAt(idx);
            return this;
        }
        
        public Pipeline RemoveHandler(IPipelineHandler handler)
        {
            for (int i = 0; i < _pipelineHandlers.Count; i++)
            {
                if (_pipelineHandlers[i].Compare(handler))
                {
                    _pipelineHandlers.RemoveAt(i);
                    break;
                }
            }
            
            return this;
        }
        
        public Pipeline RemoveHandler(Func<IPipelineHandler> factory)
        {
            for (int i = 0; i < _pipelineHandlers.Count; i++)
            {
                if (_pipelineHandlers[i].Compare(factory))
                {
                    _pipelineHandlers.RemoveAt(i);
                    break;
                }
            }
            
            return this;
        }

        public object Start(object item)
        {
            object currentItem = item;
            foreach (var factory in _pipelineHandlers)
            {
                var handler = factory.GetHandler();
                handler.Create(currentItem);

                var newElement = handler.ReturnItem();
                
                if (handler.GetError() != 0)
                {
                    onError?.Invoke(handler);
                    return null;
                }
                
                currentItem = newElement;
                
                if (newElement == null)
                {
                    return null;
                }

                onHandlerPassed?.Invoke(handler);

            }
            return currentItem;
        }
    }
}
