namespace Basement.BLFramework.Core.Common
{
    public interface IChildren<T>
    {
        T GetChild(string collectionKey);
        void AddChild(string collectionKey, T child);
        void RemoveChild(string collectionKey);
        bool Exist(string collectionKey);
    }
}
