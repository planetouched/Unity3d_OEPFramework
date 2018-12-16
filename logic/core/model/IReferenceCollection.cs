using logic.core.reference.description;

namespace logic.core.model
{
    public interface IReferenceCollection : IModel
    {
        IDescription dataSource { get; }
    }
}