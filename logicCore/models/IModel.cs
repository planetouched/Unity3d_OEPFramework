using System.Collections.Generic;
using Assets.common;
using Assets.logicCore.events;

namespace Assets.logicCore.models
{
    public interface IModel : IEventSource, ISerialize
    {
        WeakRef<IModel> weakParent { get; }

        IModel GetChild(string key);
        T GetChild<T>(string key) where T : IModel;
        IModel AddChild(string key, IModel model, bool traverseEvents = true);
        void RemoveChild(string key);
        void SetParent(IModel parent);
        List<IModel> GetModelPath();
        void Destroy();
    }
}
