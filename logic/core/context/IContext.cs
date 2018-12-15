using common;
using logic.core.model;

namespace logic.core.context
{
    public interface IContext : IChildren
    {
        RawNode repositoryNode { get; }
        T GetChild<T>(string collectionKey);
        void AddChild(string collectionKey, object obj);
    }
}
