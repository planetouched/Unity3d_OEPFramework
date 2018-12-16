using logic.core.context;
using logic.core.util;

namespace logic.core.model
{
    public abstract class CollectionBase<TModel> : ModelBase where TModel : IModel
    {
        protected CollectionBase(IContext context, IModel parent) : base(context, parent)
        {
        }

        public virtual TModel this[string collectionKey] => (TModel)GetChildren()[collectionKey];

        public override object Serialize()
        {
            var result = SerializeUtil.Dict();

            foreach (var pair in this)
            {
                var serialized = pair.Value.Serialize();

                if (serialized != null)
                {
                    result.Add(pair.Key, serialized);
                }
            }

            return result;
        }
    }
}
