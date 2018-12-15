using logic.core.reference.description;

namespace logic.core.model
{
    public interface IReferenceModel : IModel, IChildren
    {
        ISelectableDescription description { get; }
    }
}
