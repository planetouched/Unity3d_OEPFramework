namespace OEPFramework.unityEngine.behaviour
{
    public interface IControllable : IPlayable
    {
        bool initialized { get; }
        void Initialize();
        void Uninitialize();
    }
}
