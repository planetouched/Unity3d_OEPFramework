namespace Assets.logic.core.reference.description
{
    public interface ISelectableDescription : IDescription
    {
        string key { get; }
        bool canSelect { get; }
        void Initialization();
    }
}
