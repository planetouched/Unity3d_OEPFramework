using System.Collections.Generic;
using common;
using logic.core.context;
using logic.core.factories;
using logic.core.reference.dataSource;
using logic.core.reference.description;

namespace logic.core.reference.collection
{
    public class LazyArray<TValue> : DataSourceBase<int, TValue> where TValue : IDescription
    {
        public LazyArray(RawNode node, IContext context = null) : base(node, context)
        {
        }

        protected override TValue Factory(RawNode node)
        {
            if (GetContext() == null)
            {
                return FactoryManager.Build<TValue>(node);
            }

            return FactoryManager.Build<TValue>(node, GetContext());
        }

        public override TValue GetDescription(int collectionKey)
        {
            TValue value;

            if (items.TryGetValue(collectionKey, out value))
                return value;

            value = Factory(GetNode().GetNode(collectionKey));
            items.Add(collectionKey, value);

            return value;
        }

        public override IEnumerator<KeyValuePair<int, TValue>> GetEnumerator()
        {
            for (int i = 0; i < Count(); i++)
                yield return new KeyValuePair<int, TValue>(i, GetDescription(i));
        }
    }
}
