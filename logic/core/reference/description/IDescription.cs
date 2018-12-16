using common;
using logic.core.common;
using logic.core.context;
using logic.core.model;

namespace logic.core.reference.description
{
    public interface IDescription : IHasContext, IChildren<IDescription>, IParent<IDescription>
    {
        bool selectable { get; }
        string key { get; }
        RawNode GetNode();
        string GetDescriptionPath();
        void Initialization();
    }
}
