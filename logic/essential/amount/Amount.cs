using common;
using logic.core.context;
using logic.core.reference.description;

namespace logic.essential.amount
{
    public class Amount : DescriptionBase, IAmount
    {
        public string type { get; private set; }

        public Amount(RawNode node, IContext context) : base(node, context)
        {
            type = node.GetString("type");
        }

        public virtual int Number()
        {
            return 0;
        }
    }
}