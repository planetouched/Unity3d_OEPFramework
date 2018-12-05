using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.reference.description;
using Assets.logic.essential.path;

namespace Assets.logic.essential.requirement
{
    public class Requirement : DescriptionBase, IRequirement
    {
        public string type { get; private set; }
        private Path cache;

        public Requirement(RawNode node, IContext context) : base(node, context)
        {
            type = node.GetString("type");
        }

        public Path GetPath()
        {
            return cache ?? (cache = Path.Create(GetContext(), node.GetNode("path")));
        }

        public virtual bool Check()
        {
            return true;
        }
    }
}