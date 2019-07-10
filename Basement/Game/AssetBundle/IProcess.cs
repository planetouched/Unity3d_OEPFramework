using System;

namespace Basement.Game.AssetBundle
{
    public interface IProcess
    {
        float loadingProgress { get; }
        float unpackProgress { get; }
        event Action<IProcess> onProcessComplete;
        bool isComplete { get; }
    }
}
