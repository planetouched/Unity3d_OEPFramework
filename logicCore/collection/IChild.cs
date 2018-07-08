using Assets.common;

namespace Assets.logicCore.collection
{
    public interface IChild
    {
        string id { get; }
        WeakRef<IChild> weakParent { get; }

        IChild GetChild(string key);
        T GetChild<T>(string key) where T : IChild;
        IChild AddChild(string key, IChild obj, bool traverseEvents = true);
        void RemoveChild(string key);
        void SetParent(IChild parent);
    }
}
