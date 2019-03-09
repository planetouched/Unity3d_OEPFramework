using common;
using logic.core.context;
using logic.core.reference.description;

namespace logic.core.model
{
    public abstract class ReferenceModelBase<TCategories, TDescription> : ModelBase, IReferenceModel
        where TDescription : IDescription
        where TCategories : class
    {
        public TDescription description { get; }
        public TCategories categories { get; protected set; }

        public bool selectable { get; }
        protected readonly RawNode initNode;

        protected ReferenceModelBase(RawNode initNode, TCategories categories, TDescription description, IContext context) : base(context)
        {
            key = description.key;
            this.initNode = initNode;
            this.description = description;
            this.categories = categories;
            selectable = description.selectable;
        }
    }
}