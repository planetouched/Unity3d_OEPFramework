using System.Collections.Generic;

namespace OEPCommon.AssetBundles
{
    public class Unloader
    {
        private readonly List<(string assetBundleName, bool withDependencies)> _counter = new List<(string assetBundleName, bool withDependencies)>();

        public void Add(string resource, bool withDependencies)
        {
            _counter.Add((resource, withDependencies));
        }

        public void Unload()
        {
            foreach (var pair in _counter)
            {
                AssetBundleManager.TryUnload(pair.assetBundleName, pair.withDependencies);
            }
            _counter.Clear();
        }
    }
}
