using System;

namespace game.assetBundle
{
    public interface IProcess
    {
        float loadingProgress { get; }
        float unpackProgress { get; }
        event Action<IProcess> onProcessComplete;
        bool isComplete { get; }
    }
}
