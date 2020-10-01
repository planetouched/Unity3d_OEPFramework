using System.Collections;
using System.Collections.Generic;
using Basement.Common;
using UnityEngine;

namespace OEPCommon.AssetBundles.Repository
{
    public class AssetBundlesRepository : IEnumerable<KeyValuePair<string, AssetBundleRepositoryItem>>
    {
        private readonly Dictionary<string, AssetBundleRepositoryItem> _repositoryItems = new Dictionary<string, AssetBundleRepositoryItem>();

        public AssetBundleRepositoryItem this[string key]
        {
            get
            {
                if (!_repositoryItems.ContainsKey(key))
                {
                    Debug.LogError("Error with key: " + key);
                }

                return _repositoryItems[key];
            }
        }

        public AssetBundlesRepository(RawNode node)
        {
            foreach (var dict in node.GetUnsortedCollection())
            {
                _repositoryItems.Add(dict.Key, new AssetBundleRepositoryItem(dict.Value));
            }
        }

        public IEnumerator<KeyValuePair<string, AssetBundleRepositoryItem>> GetEnumerator()
        {
            foreach (var pair in _repositoryItems)
            {
                yield return pair;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}