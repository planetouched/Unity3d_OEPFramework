using System.Collections;
using System.Collections.Generic;
using common;
using logic.core.context;
using logic.core.factories;
using logic.core.reference.description;

namespace logic.core.reference.collection
{
    public abstract class LazyCollectionBase<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>> where TValue : IDescription
    {
        private readonly WeakRef<IContext> _weakContext;
        protected readonly RawNode descriptionNode;
        protected readonly IDictionary<TKey, TValue> items = new Dictionary<TKey, TValue>();
        
        public TValue this[TKey collectionKey] => GetDescription(collectionKey);

        protected LazyCollectionBase(RawNode descriptionNode, IContext context = null)
        {
            this.descriptionNode = descriptionNode;

            if (context != null)
            {
                _weakContext = new WeakRef<IContext>(context);
            }        
        }
        
        protected virtual TValue Factory(RawNode partialNode)
        {
            if (GetContext() == null)
            {
                return FactoryManager.Build<TValue>(partialNode);
            }

            return FactoryManager.Build<TValue>(partialNode, GetContext());
        }

        protected abstract TValue GetDescription(TKey collectionKey);

        public abstract IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IContext GetContext()
        {
            return _weakContext == null ? null : _weakContext.obj;
        }
    }
}