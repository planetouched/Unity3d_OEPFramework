using System.Collections.Generic;
using OEPFramework.common;

namespace OEPFramework.unityEngine.assetBundle.repository
{
    public class AssetBundlesRepository
    {
        readonly Dictionary<string, AssetBundleRepositoryItem> repositoryItems = new Dictionary<string, AssetBundleRepositoryItem>();

        public AssetBundleRepositoryItem this[string key]
        {
            get { return repositoryItems[key]; }
        }

        public AssetBundlesRepository(RawNode node)
        {
            foreach (var dict in node.GetUnsortedCollection())
            {
                repositoryItems.Add(dict.Key, new AssetBundleRepositoryItem(dict.Value)); 
            }
        }
    }
}
