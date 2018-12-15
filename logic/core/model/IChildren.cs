namespace logic.core.model
{
    public interface IChildren
    {
        IModel GetChild(string collectionKey);
        void AddChild(string collectionKey, IModel model);
        void RemoveChild(string collectionKey);
        bool Exist(string collectionKey);
        int Count();
    }
}
