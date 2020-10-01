using System;
using System.Collections.Generic;
using System.IO;
using Basement.Common;
using Basement.OEPFramework.Futures;
using Basement.OEPFramework.Futures.Util;
using Basement.OEPFramework.UnityEngine.Util;
using OEPCommon.AssetBundles.Futures;
using OEPCommon.AssetBundles.Repository;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace OEPCommon.AssetBundles
{
    public static class AssetBundleManager
    {
        private class AssetBundleRef
        {
            public Object[] allAssets { get; set; }
            public AssetBundle assetBundle { get; set; }
            public int loadedCount { get; set; }
            public bool dependency { get; }

            public AssetBundleRef(bool dependency)
            {
                this.dependency = dependency;
                loadedCount = 1;
            }

            public T FindAsset<T>(string assetName) where T : Object
            {
                foreach (var asset in allAssets)
                {
                    if (String.Equals(asset.name, assetName, StringComparison.CurrentCultureIgnoreCase) && asset is T)
                    {
                        return (T) asset;
                    }
                }

                return null;
            }
        }

        private class DependencyNode
        {
            public DependencyNode parentNode { get; }
            public string assetBundleName { get; }
            public List<DependencyNode> nodes { get; } = new List<DependencyNode>();
            public bool stop { get; set; }

            public DependencyNode(string assetBundleName, DependencyNode parent)
            {
                parentNode = parent;
                this.assetBundleName = assetBundleName;
            }
        }

        public static int loadedPackedSize { get; private set; }
        public static AssetBundlesRepository repository { get; private set; }
        public static string storageUrl { get; private set; }
        
        private static readonly Dictionary<string, LoadAssetBundlePromise> _loading = new Dictionary<string, LoadAssetBundlePromise>();
        private static readonly Dictionary<string, AssetBundleRef> _assetBundlesRefs = new Dictionary<string, AssetBundleRef>();

        public static Action<string> onBundleCompleted { get; set; } 
        public static Action<string> onBundleUnload { get; set; } 

        public static void Init(RawNode repositoryNode, string assetBundlesUrl)
        {
            AssetBundleManager.storageUrl = assetBundlesUrl;
            repository = new AssetBundlesRepository(repositoryNode);
        }

        public static T GetAsset<T>(string bundle, string assetName) where T : Object
        {
            return _assetBundlesRefs[bundle].FindAsset<T>(assetName);
        }
        
        public static T GetAsset<T>(string assetBundle, char separator = ':') where T : Object
        {
            var arr = assetBundle.Split(separator);
            return _assetBundlesRefs[arr[0]].FindAsset<T>(arr[1]);
        }

        public static string GetBundleName(string assetBundle, char separator = ':')
        {
            return assetBundle.Split(separator)[0];
        }
        
        public static Object[] GetAllAssets(string assetBundle)
        {
            return _assetBundlesRefs[assetBundle].allAssets;
        }

        public static int GetLoadedCount(string assetBundle)
        {
            return _assetBundlesRefs[assetBundle].loadedCount;
        }

        public static AsyncOperation UnloadUnused()
        {
            List<string> remove = new List<string>();
            foreach (var pair in _assetBundlesRefs)
            {
                if (pair.Value.loadedCount == 0)
                {
                    pair.Value.allAssets = null;
                    
                    if (pair.Value.dependency)
                    {
                        pair.Value.assetBundle.Unload(true);
                    }
                    loadedPackedSize -= repository[pair.Key].packedSize;
                    remove.Add(pair.Key);
                    onBundleUnload?.Invoke(pair.Key);
                    
                    Debug.Log("Unload: " + pair.Key);
                }
            }

            foreach (var key in remove)
            {
                _assetBundlesRefs.Remove(key);
            }

            return Resources.UnloadUnusedAssets();
        }

        public static void TryUnload(string mainAssetBundle, bool withDependencies)
        {
            var node = new DependencyNode(mainAssetBundle, null);
            
            if (withDependencies)
            {
                CollectDependencies(node);
            }
            
            var assetBundles = new List<DependencyNode>();
            
            while (!GetLatestDependencies(node, assetBundles))
            {
                foreach (var assetBundle in assetBundles)
                {
                    var assetBundleRef = _assetBundlesRefs[assetBundle.assetBundleName];
                    assetBundleRef.loadedCount--;
                    if (assetBundleRef.loadedCount < 0)
                        throw new Exception("loadedCount < 0, assetBundle: " + assetBundle.assetBundleName);
                }

                assetBundles.Clear();
            }
        }

        public static void ClearOldCachedVersions(List<string> assetBundleNames = null)
        {
            var list = new List<(string hash, string fileName)>();

            if (assetBundleNames != null)
            {
                foreach (var assetBundleName in assetBundleNames)
                {
                    list.Add((assetBundleName, repository[assetBundleName].hash));
                }
            }
            else
            {
                foreach (var pair in repository)
                {
                    list.Add((pair.Key, pair.Value.hash));
                }
            }
            
            AssetBundleCache.ClearOtherCachedVersions(list);
        }
        
        public static bool IsCached(string assetBundleName)
        {
            return AssetBundleCache.IsCached(assetBundleName, repository[assetBundleName].hash);
        }

        public static bool IsLoaded(string assetBundleName)
        {
            return _assetBundlesRefs.ContainsKey(assetBundleName);
        }

        public static bool IsDependency(string assetBundleName)
        {
            return repository[assetBundleName].isDependency;
        }

        public static AssetBundleWebRequestFuture DownloadFromWWW(string assetBundleName, bool runImmediately = true)
        {
            if (IsCached(assetBundleName)) return null;
            Debug.Log("Download WWW: " + assetBundleName);
            var node = repository[assetBundleName];
            
            var requestFuture =  new AssetBundleWebRequestFuture(storageUrl, node.file, false, node.crc32, Int32.MaxValue);
            
            requestFuture.AddListener(f =>
            {
                if (f.isDone)
                {
                    AssetBundleCache.AddCache(assetBundleName, node.hash, f.Cast<AssetBundleWebRequestFuture>().result, false);
                }
            });
            
            if (runImmediately)
            {
                requestFuture.Run();
            }
            
            return requestFuture;
        }

        private static void LoadPartial(CascadeLoading cascade, List<IProcess> processList, IEnumerable<DependencyNode> assetBundles, bool async)
        {
            foreach (var assetBundleNode in assetBundles)
            {
                string assetBundleName = assetBundleNode.assetBundleName;
                
                if (_loading.ContainsKey(assetBundleName))
                {
                    //loading at this moment
                    var loader = _loading[assetBundleName];
                    cascade.AddFuture(loader);

                    processList?.Add(loader);
                    _assetBundlesRefs[assetBundleName].loadedCount++;
                }
                else
                {
                    if (_assetBundlesRefs.ContainsKey(assetBundleName) && !_loading.ContainsKey(assetBundleName))
                    {
                        //already downloaded
                        _assetBundlesRefs[assetBundleName].loadedCount++;
                        continue;
                    }

                    //first load
                    var node = repository[assetBundleName];
                    var loader = new LoadAssetBundlePromise(node.file, storageUrl, node.isDependency, node.crc32, node.hash, async);
                    _loading.Add(assetBundleName, loader);
                    _assetBundlesRefs.Add(assetBundleName, new AssetBundleRef(node.isDependency));

                    loader.AddListener(future =>
                    {
                        _loading.Remove(assetBundleName);
                        loadedPackedSize += repository[assetBundleName].packedSize;
                        var ab = _assetBundlesRefs[assetBundleName];
                        ab.allAssets = loader.GetAssets();
                        ab.assetBundle = loader.assetBundle;
                        
                        onBundleCompleted?.Invoke(assetBundleName);
                    });

                    cascade.AddFuture(loader);
                    processList?.Add(loader);
                }
            }
        }

        public static IFuture Load(string assetBundleName, out List<IProcess> processList, bool async = true, bool withDependencies = true)
        {
            var cascade = new CascadeLoading();
            var node = new DependencyNode(assetBundleName, null);
            var assetBundles = new List<DependencyNode>();
            processList = new List<IProcess>();

            if (withDependencies)
            {
                CollectDependencies(node);
            
                while (!GetLatestDependencies(node, assetBundles))
                {
                    LoadPartial(cascade, processList, assetBundles, async);
                    assetBundles.Clear();
                    cascade.Next();
                }

                cascade.Trim();
            }
            else
            {
                LoadPartial(cascade, processList, new [] {node}, async);
            }
            
            return new CascadeLoadingPromise(cascade);
        }

        private static bool GetLatestDependencies(DependencyNode currentNode, List<DependencyNode> list)
        {
            if (currentNode.stop) 
            {
                return true;
            }

            if (currentNode.nodes.Count == 0)
            {
                list.Add(currentNode);

                if (currentNode.parentNode != null)
                {
                    currentNode.parentNode.nodes.Remove(currentNode);
                }
                else
                {
                    currentNode.stop = true;
                }
            }
            else
            {
                var copyCollection = new List<DependencyNode>(currentNode.nodes);
                
                foreach (var node in copyCollection)
                {
                    GetLatestDependencies(node, list);
                }
            }

            return false;
        }

        private static void CollectDependencies(DependencyNode parentNode)
        {
            foreach (var name in repository[parentNode.assetBundleName].dependencies)
            {
                var node = new DependencyNode(name, parentNode);
                parentNode.nodes.Add(node);
                CollectDependencies(node);
            }
        }
    }
}
