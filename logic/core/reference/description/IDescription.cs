using common;
using logic.core.context;

namespace logic.core.reference.description
{
    public interface IDescription : IHasContext
    {
        RawNode GetNode();
    }
}
