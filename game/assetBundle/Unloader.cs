using System.Collections.Generic;
using common.utils;

namespace Assets.game.assetBundle
{
    public class Unloader
    {
        readonly Dictionary<string, int> counter = new Dictionary<string, int>();
        private readonly AssetBundleManager manager;

        public Unloader()
        {
            manager = SingletonManager.Get<AssetBundleManager>();
        }
        public void Add(string resource)
        {
            int count;
            if (counter.TryGetValue(resource, out count))
                counter[resource] = ++count;
            else
                counter.Add(resource, 1);
        }

        public void Unload()
        {
            foreach (var pair in counter)
            {
                for (int i = 0; i < pair.Value; i++)
                    manager.TryUnload(pair.Key);
            }
            counter.Clear();
        }
    }
}
