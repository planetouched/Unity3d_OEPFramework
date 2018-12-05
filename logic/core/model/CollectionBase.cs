using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Assets.logic.core.context;

namespace Assets.logic.core.model
{
    public class CollectionBase<TModel> : ModelBase, IChildren where TModel : IModel
    {
        private IOrderedDictionary children;

        public CollectionBase(IContext context, IModel parent) : base(context, parent)
        {
        }

        public virtual TModel this[string collectionKey]
        {
            get
            {
                return (TModel)InnerGetChildren()[collectionKey];
            }
        }

        private IDictionary InnerGetChildren()
        {
            return children ?? (children = new OrderedDictionary());
        }

        public void AddChild(string key, IModel model)
        {
            InnerGetChildren().Add(key, model);
            model.SetParent(this);
        }

        public virtual IModel GetChild(string key)
        {
            return (IModel)InnerGetChildren()[key];
        }

        public void RemoveChild(string key)
        {
            if (children == null) return;

            if (children.Contains(key))
            {
                GetChild(key).SetParent(null);
                children.Remove(key);
            }
        }

        public bool Exist(string key)
        {
            return children.Contains(key);
        }

        public int Count()
        {
            return children.Count;
        }

        public IEnumerable<KeyValuePair<string, TModel>> GetCollection()
        {
            if (children != null)
            {
                foreach (DictionaryEntry pair in children)
                {
                    yield return new KeyValuePair<string, TModel>((string) pair.Key, (TModel) pair.Value);
                }
            }
        }

        public override object Serialize()
        {
            var result = new Dictionary<string, object>();

            foreach (var pair in GetCollection())
            {
                object serialized = pair.Value.Serialize();

                if (serialized != null)
                {
                    result.Add(pair.Key, serialized);
                }
            }

            return result;
        }

        public override void Destroy()
        {
            base.Destroy();

            if (children == null) return;

            foreach (var pair in GetCollection())
            {
                pair.Value.Destroy();
            }

            children.Clear();
        }
    }
}
