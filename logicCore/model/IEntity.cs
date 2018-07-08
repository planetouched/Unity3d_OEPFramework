using Assets.logicCore.collection;
using Assets.logicCore.throughEvent;

namespace Assets.logicCore.model
{
    public interface IEntity : IEventSource, IChild, ISerialize
    {
        void Destroy();
    }
}
