﻿using System.Collections.Generic;
using common;

namespace game.assetBundle.repository
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
