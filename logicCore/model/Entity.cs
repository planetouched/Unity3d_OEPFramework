using System.Collections;
using System.Collections.Generic;

namespace Assets.logicCore.model
{
    public abstract class Entity : EventSource, IEntity
    {
        protected Entity(string id) : base(id)
        {
        }

        public IEnumerable<KeyValuePair<string, IEntity>> GetCollection()
        {
            if (children != null)
            {
                foreach (DictionaryEntry pair in children)
                    yield return new KeyValuePair<string, IEntity>((string)pair.Key, (IEntity)pair.Value);
            }
        }

        public object Serialize()
        {
            var result = new Dictionary<string, object>();
            foreach (var pair in GetCollection())
            {
                object serialized = pair.Value.Serialize();
                if (serialized != null)
                    result.Add(pair.Key, serialized);
            }
            return result;
        }

        public virtual void Destroy()
        {
            if (throughEvent != null)
                throughEvent.Clear();

            SetParent(null);
            if (children == null) return;
            foreach (var pair in GetCollection())
            {
                pair.Value.Destroy();
            }

            children.Clear();
        }
    }
}
