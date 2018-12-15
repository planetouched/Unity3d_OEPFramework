using System.Collections.Generic;
using common;
using logic.core.context;

namespace logic.core.reference.dataSource
{
    public interface IDataSource<TKey, TDescription> : IHasContext, IEnumerable<KeyValuePair<TKey, TDescription>>
    {
        string key { get; }
        TDescription GetDescription(TKey collectionKey);
        RawNode GetNode();
    }
}
