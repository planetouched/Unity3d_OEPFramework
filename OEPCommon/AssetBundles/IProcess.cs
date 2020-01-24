using System;

namespace OEPCommon.AssetBundles
{
    public interface IProcess
    {
        float loadingProgress { get; }
        event Action<IProcess> onProcessComplete;
        bool isComplete { get; }

        void Cancel();
    }
}
