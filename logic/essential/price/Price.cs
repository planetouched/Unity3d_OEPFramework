using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.description;
using Assets.logic.essential.path;

namespace Assets.logic.essential.price
{
    public class Price : DescriptionBase, IPrice
    {
        public string type { get; private set; }
        public int amount { get; private set; }
        private Path cache;

        public Price(RawNode node, IContext context) : base(node, context)
        {
            type = node.GetString("type");
            amount = node.GetInt("amount");
        }

        public Path GetPath()
        {
            return cache ?? (cache = Path.Create(GetContext(), node.GetNode("path")));
        }

        public virtual bool Check()
        {
            return true;
        }

        public virtual void Pay()
        {
        }
    }
}
