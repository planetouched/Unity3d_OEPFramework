using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.description;
using Assets.logic.core.throughEvent;
using Assets.logic.essential.path;

namespace Assets.logic.essential.requirement
{
    public class Requirement : DescriptionBase, IRequirement
    {
        public string type { get; private set; }
        private ModelsPath cache;

        public Requirement(RawNode node, IContext context) : base(node, context)
        {
            type = node.GetString("type");
        }

        public ModelsPath GetPath()
        {
            return cache ?? (cache = PathUtil.ModelsPath(GetContext(), node.GetString("path"), null));
        }

        public virtual bool Check()
        {
            return true;
        }
    }
}