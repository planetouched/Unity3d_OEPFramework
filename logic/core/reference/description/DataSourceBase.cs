using System.Collections;
using System.Collections.Generic;
using common;
using logic.core.context;

namespace logic.core.reference.description
{
    public abstract class DataSourceBase<TDescription> : DescriptionBase, IEnumerable<KeyValuePair<string, TDescription>> where TDescription : IDescription
    {
        protected abstract TDescription Factory(RawNode node);
        
        private WeakRef<IDescription> weakParent;
        
        public TDescription this[string collectionKey] => GetDescription(collectionKey);

        protected DataSourceBase(RawNode node, IContext context = null) : base(node, context)
        {
        }

        private TDescription GetDescription(string collectionKey)
        {
            IDescription value;

            if (GetChildren().TryGetValue(collectionKey, out value))
                return (TDescription)value;

            value = Factory(node.GetNode(collectionKey));
            AddChild(collectionKey, value);
            return (TDescription)value;            
        }

        public IEnumerator<KeyValuePair<string, TDescription>> GetEnumerator()
        {
            foreach (var pair in node.GetSortedCollection())
            {
                yield return new KeyValuePair<string, TDescription>(pair.Key, GetDescription(pair.Key));
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override IDescription GetChild(string collectionKey)
        {
            return GetDescription(collectionKey);
        }
    }
}