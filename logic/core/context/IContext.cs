using Assets.logic.core.model;

namespace Assets.logic.core.context
{
    public interface IContext : IChildren
    {
        T GetChild<T>(string collectionKey);
        void AddChild(string collectionKey, object obj);
    }
}
