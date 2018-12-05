using System.Collections.Generic;
using Assets.common;
using Assets.logic.core.context;

namespace Assets.logic.core.reference.dataSource
{
    public interface IDataSource<TKey, TDescription> : IHasContext, IEnumerable<KeyValuePair<TKey, TDescription>>
    {
        string key { get; }
        TDescription GetDescription(TKey collectionKey);
        RawNode GetNode();
    }
}
