using System.Collections.Generic;

namespace OEPCommon.AssetBundles
{
    public class Unloader
    {
        private readonly Dictionary<string, int> _counter = new Dictionary<string, int>();

        public void Add(string resource)
        {
            if (_counter.TryGetValue(resource, out var count))
            {
                _counter[resource] = ++count;
            }
            else
            {
                _counter.Add(resource, 1);
            }
        }

        public void Unload()
        {
            foreach (var pair in _counter)
            {
                for (int i = 0; i < pair.Value; i++)
                {
                    AssetBundleManager.TryUnload(pair.Key);
                }
            }
            _counter.Clear();
        }
    }
}
