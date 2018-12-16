using logic.core.common;

namespace logic.core.model
{
    public interface IReferenceModel : IModel, IChildren<IModel>
    {
        bool selectable { get; }
    }
}
