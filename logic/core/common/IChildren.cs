namespace logic.core.common
{
    public interface IChildren<T>
    {
        T GetChild(string collectionKey);
        void AddChild(string collectionKey, T obj);
        void RemoveChild(string collectionKey, bool destroy);
        bool Exist(string collectionKey);
    }
}
