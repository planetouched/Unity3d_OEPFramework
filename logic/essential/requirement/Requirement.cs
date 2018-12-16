using common;
using logic.core.context;
using logic.core.reference.description;
using logic.core.throughEvent;
using logic.essential.path;

namespace logic.essential.requirement
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
            return cache ?? (cache = PathUtil.GetModelPath(GetContext(), node.GetString("path"), null));
        }

        public virtual bool Check()
        {
            return true;
        }
    }
}