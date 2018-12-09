using System.Collections;
using System.Collections.Generic;
using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.description;

namespace Assets.logic.core.reference.dataSource
{
    public abstract class DataSourceBase<TKey, TDescription> : IDataSource<TKey, TDescription> where TDescription : IDescription
    {
        private readonly WeakRef<IContext> weakContext;
        protected IDictionary<TKey, TDescription> items = new Dictionary<TKey, TDescription>();
        protected abstract TDescription Factory(RawNode node);
        private readonly RawNode node;
        public string key { get; private set; }

        public TDescription this[TKey collectionKey]
        {
            get { return GetDescription(collectionKey); }
        }

        protected DataSourceBase(RawNode node, IContext context = null)
        {
            this.node = node;
            key = node.nodeKey;

            if (context != null)
            {
                weakContext = new WeakRef<IContext>(context);
            }
        }

        public abstract IEnumerator<KeyValuePair<TKey, TDescription>> GetEnumerator();

        public abstract TDescription GetDescription(TKey collectionKey);

        public int Count()
        {
            return GetNode().nodesCount;
        }

        public RawNode GetNode()
        {
            return node;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IContext GetContext()
        {
            if (weakContext == null)
            {
                return null;
            }

            return weakContext.obj;
        }
    }
}