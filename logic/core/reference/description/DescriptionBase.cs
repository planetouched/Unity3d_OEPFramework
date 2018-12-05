using Assets.common;
using Assets.logic.core.context;

namespace Assets.logic.core.reference.description
{
    public abstract class DescriptionBase : IDescription
    {
        protected RawNode node;
        private readonly WeakRef<IContext> weakContext;

        protected DescriptionBase(RawNode node, IContext context = null)
        {
            this.node = node;

            if (context != null)
            {
                weakContext = new WeakRef<IContext>(context);
            }
        }

        public RawNode GetNode()
        {
            return node;
        }

        public IContext GetContext()
        {
            return weakContext.obj;
        }
    }
}
