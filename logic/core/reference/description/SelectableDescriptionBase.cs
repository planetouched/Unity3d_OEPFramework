using Assets.common;
using Assets.logic.core.context;

namespace Assets.logic.core.reference.description
{
    public abstract class SelectableDescriptionBase : DescriptionBase, ISelectableDescription
    {
        public string key { get; private set; }
        public bool canSelect { get; private set; }

        protected SelectableDescriptionBase(RawNode node, IContext context = null) : base(node, context)
        {
            key = node.nodeKey;
            canSelect = node.GetBool("select", true);
        }

        public virtual void Initialization()
        {
        }
    }
}
