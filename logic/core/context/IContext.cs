using common;
using game.model;
using logic.core.common;
using logic.core.model;

namespace logic.core.context
{
    public interface IContext: IChildren<IModel>
    {
        DataSources dataSources { get; }
        RawNode repositoryNode { get; }
        T GetChild<T>(string collectionKey) where T : class;
    }
}
