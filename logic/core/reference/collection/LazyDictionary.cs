using System.Collections.Generic;
using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.factories;
using Assets.logic.core.reference.dataSource;
using Assets.logic.core.reference.description;

namespace Assets.logic.core.reference.collection
{
    public class LazyDictionary<TValue> : DataSourceBase<string, TValue> where TValue : IDescription
    {
        public LazyDictionary(RawNode node, IContext context = null) : base(node, context)
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

        public override TValue GetDescription(string collectionKey)
        {
            TValue value;

            if (items.TryGetValue(collectionKey, out value))
                return value;

            value = Factory(GetNode().GetNode(collectionKey));
            items.Add(collectionKey, value);

            return value;
        }

        public override IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            foreach (var item in items)
            {
                yield return item;
            }
        }
    }
}
