using System.Collections.Generic;
using logic.core.common;
using logic.core.reference.description;

namespace logic.core.context
{
    public class DataSources : IChildren<IDescription>
    {
        private readonly IDictionary<string, IDescription> _children = new Dictionary<string, IDescription>();
        
       
        public IDescription GetChild(string collectionKey)
        {
            return _children[collectionKey];
        }

        public void AddChild(string collectionKey, IDescription child)
        {
            _children.Add(collectionKey, child);
        }

        public void RemoveChild(string collectionKey)
        {
            throw new System.NotImplementedException();
        }

        public bool Exist(string collectionKey)
        {
            return _children.ContainsKey(collectionKey);
        }
        
        public T GetChild<T>(string collectionKey) where T : class
        {
            return (T)_children[collectionKey];
        }
    }
}