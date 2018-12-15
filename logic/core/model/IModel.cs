using System.Collections.Generic;
using logic.core.context;
using logic.core.throughEvent;

namespace logic.core.model
{
    public interface IModel : IEventSource, ISerialize, IHasContext
    {
        string key { get; }
        void Initialization();
        void SetParent(IModel newParent);
        IModel GetParent();
        IList<IModel> GetModelPath(bool check);
        bool CheckAvailable();
        void Destroy();
    }
}
