using System.Collections.Generic;
using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.description;

namespace Assets.logic.core.reference.dataSource
{
    public abstract class DataSourceSelectableBase<TDescription> : DataSourceBase<string, TDescription> where TDescription : ISelectableDescription
    {
        private KeyValuePair<string, TDescription>[] sortedCache;

        protected DataSourceSelectableBase(RawNode node, IContext context = null) : base(node, context)
        {
        }

        public IEnumerable<string> GetUnsortedKeys()
        {
            foreach (var pair in GetNode().GetUnsortedCollection())
                yield return pair.Key;
        }

        public IEnumerable<string> GetSortedKeys()
        {
            foreach (var pair in GetNode().GetSortedCollection())
                yield return pair.Key;
        }

        public override TDescription GetDescription(string collectionKey)
        {
            TDescription value;

            if (items.TryGetValue(collectionKey, out value))
                return value;

            value = Factory(GetNode().GetNode(collectionKey));
            items.Add(collectionKey, value);
            //value.Initialization();
            sortedCache = null;

            return value;
        }

        public override IEnumerator<KeyValuePair<string, TDescription>> GetEnumerator()
        {
            if (sortedCache == null)
            {
                int idx = 0;
                sortedCache = new KeyValuePair<string, TDescription>[GetNode().nodesCount];
                foreach (var collectionKey in GetSortedKeys())
                    sortedCache[idx++] = new KeyValuePair<string, TDescription>(collectionKey, GetDescription(collectionKey));
            }

            foreach (var pair in sortedCache)
                yield return pair;
        }
    }
}
