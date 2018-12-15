using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using logic.core.context;
using logic.core.util;

namespace logic.core.model
{
    public class CollectionBase<TModel> : ModelBase, IEnumerable<KeyValuePair<string, TModel>>, IChildren where TModel : IModel
    {
        private IOrderedDictionary children;

        public CollectionBase(IContext context, IModel parent) : base(context, parent)
        {
        }

        public virtual TModel this[string collectionKey]
        {
            get { return (TModel)InnerGetChildren()[collectionKey]; }
        }

        private IDictionary InnerGetChildren()
        {
            return children ?? (children = new OrderedDictionary());
        }

        public void AddChild(string collectionKey, IModel model)
        {
            InnerGetChildren().Add(collectionKey, model);
            model.SetParent(this);
        }

        public virtual IModel GetChild(string collectionKey)
        {
            return (IModel)InnerGetChildren()[collectionKey];
        }

        public void RemoveChild(string collectionKey)
        {
            if (children == null) return;

            if (children.Contains(collectionKey))
            {
                GetChild(collectionKey).SetParent(null);
                children.Remove(collectionKey);
            }
        }

        public bool Exist(string collectionKey)
        {
            return children.Contains(collectionKey);
        }

        public int Count()
        {
            return children.Count;
        }

        public override object Serialize()
        {
            var result = SerializeUtil.Dict();

            foreach (var pair in GetOnlyInitializedCollectionItems())
            {
                var serialized = pair.Value.Serialize();

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

            foreach (var pair in GetOnlyInitializedCollectionItems())
            {
                pair.Value.Destroy();
            }

            children.Clear();
        }

        private IEnumerable<KeyValuePair<string, TModel>> GetOnlyInitializedCollectionItems()
        {
            if (children != null)
            {
                foreach (TModel item in children)
                {
                    yield return new KeyValuePair<string, TModel>(item.key, item);
                }
            }
        }

        public virtual IEnumerator<KeyValuePair<string, TModel>> GetEnumerator()
        {
            foreach (var pair in GetOnlyInitializedCollectionItems())
            {
                yield return pair;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
