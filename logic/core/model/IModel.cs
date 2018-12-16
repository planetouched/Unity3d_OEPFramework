using System.Collections.Generic;
using logic.core.common;
using logic.core.context;
using logic.core.throughEvent;

namespace logic.core.model
{
    public interface IModel : IEventSource, ISerialize, IHasContext, IChildren<IModel>, IParent<IModel>
    {
        string key { get; }
        void Initialization();
        IList<IModel> GetModelPath(bool check);
        bool CheckAvailable();
        void Destroy();
    }
}
