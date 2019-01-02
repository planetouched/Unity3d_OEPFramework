using System;

namespace game.assetBundle
{
    public interface IProcess
    {
        float loadingProgress { get; }
        float unpackProgress { get; }
        Action<IProcess> onProcessComplete { get; }
        bool isComplete { get; }
    }
}
