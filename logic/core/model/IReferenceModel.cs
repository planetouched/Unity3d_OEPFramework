using Assets.logic.core.reference.description;

namespace Assets.logic.core.model
{
    public interface IReferenceModel : IModel, IChildren
    {
        ISelectableDescription description { get; }
    }
}
