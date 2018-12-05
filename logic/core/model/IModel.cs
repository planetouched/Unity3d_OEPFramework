using System.Collections.Generic;
using Assets.logic.core.context;
using Assets.logic.core.throughEvent;

namespace Assets.logic.core.model
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
