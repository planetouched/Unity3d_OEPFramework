using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.description;
using Assets.logic.core.throughEvent;
using Assets.logic.essential.path;

namespace Assets.logic.essential.price
{
    public class Price : DescriptionBase, IPrice
    {
        public string type { get; private set; }
        public int amount { get; private set; }
        private ModelsPath cache;

        public Price(RawNode node, IContext context) : base(node, context)
        {
            type = node.GetString("type");
            amount = node.GetInt("amount");
        }

        public ModelsPath GetPath()
        {
            return cache ?? (cache = PathUtil.ModelsPath(GetContext(), node.GetString("path"), null));
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
