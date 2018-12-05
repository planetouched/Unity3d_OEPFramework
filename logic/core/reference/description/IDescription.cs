using Assets.common;
using Assets.logic.core.context;

namespace Assets.logic.core.reference.description
{
    public interface IDescription : IHasContext
    {
        RawNode GetNode();
    }
}
