using System.Collections.Generic;
using Basement.Common;

namespace Game.AssetBundles.Repository
{
    public class AssetBundlesRepository
    {
        readonly Dictionary<string, AssetBundleRepositoryItem> repositoryItems = new Dictionary<string, AssetBundleRepositoryItem>();

        public AssetBundleRepositoryItem this[string key] => repositoryItems[key];

        public AssetBundlesRepository(RawNode node)
        {
            foreach (var dict in node.GetUnsortedCollection())
            {
                repositoryItems.Add(dict.Key, new AssetBundleRepositoryItem(dict.Value)); 
            }
        }
    }
}
