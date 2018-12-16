using System.Collections.Generic;
using common;
using logic.core.context;
using logic.core.reference.description;

namespace logic.core.reference.collection
{
    public class LazyArray<TValue> : LazyCollectionBase<int, TValue> where TValue : IDescription
    {
        public LazyArray(RawNode descriptionNode, IContext context = null) : base(descriptionNode, context)
        {
        }

        protected override TValue GetDescription(int collectionKey)
        {
            TValue value;

            if (items.TryGetValue(collectionKey, out value))
                return value;

            value = Factory(descriptionNode.GetNode(collectionKey));
            items.Add(collectionKey, value);

            return value;
        }

        public int Count()
        {
            return descriptionNode.nodesCount;
        }

        public override IEnumerator<KeyValuePair<int, TValue>> GetEnumerator()
        {
            for (int i = 0; i < descriptionNode.nodesCount; i++)
            {
                yield return new KeyValuePair<int, TValue>(i, GetDescription(i));
            }
        }
    }
}
