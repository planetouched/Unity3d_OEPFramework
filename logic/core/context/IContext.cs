using Assets.common;
using Assets.logic.core.model;

namespace Assets.logic.core.context
{
    public interface IContext : IChildren
    {
        RawNode repositoryNode { get; }
        T GetChild<T>(string collectionKey);
        void AddChild(string collectionKey, object obj);
    }
}
