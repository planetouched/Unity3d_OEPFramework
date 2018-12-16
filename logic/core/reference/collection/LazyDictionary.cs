﻿using System.Collections.Generic;
using common;
using logic.core.context;
using logic.core.reference.description;

namespace logic.core.reference.collection
{
    public class LazyDictionary<TValue> : LazyCollectionBase<string, TValue> where TValue : IDescription
    {
        public LazyDictionary(RawNode node, IContext context = null) : base(node, context)
        {
        }

        protected override TValue GetDescription(string collectionKey)
        {
            TValue value;

            if (items.TryGetValue(collectionKey, out value))
                return value;

            value = Factory(descriptionNode.GetNode(collectionKey));
            items.Add(collectionKey, value);

            return value;
        }

        public override IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            foreach (var collectionKey in descriptionNode.GetSortedKeys())
            {
                yield return new KeyValuePair<string, TValue>(collectionKey, GetDescription(collectionKey)); 
            }
        }
    }
}
