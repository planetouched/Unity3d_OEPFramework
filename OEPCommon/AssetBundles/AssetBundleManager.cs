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
            public UnityWebRequest request { get; set; }
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
        public static string assetBundlesUrl { get; private set; }
        
        private static readonly Dictionary<string, LoadAssetBundlePromise> _loading = new Dictionary<string, LoadAssetBundlePromise>();
        private static readonly Dictionary<string, AssetBundleRef> _assetBundlesRefs = new Dictionary<string, AssetBundleRef>();

        public static Action<string> onBundleCompleted { get; set; } 
        public static Action<string> onBundleUnload { get; set; } 

        public static void Init(RawNode repositoryNode, string assetBundlesUrl)
        {
            AssetBundleManager.assetBundlesUrl = assetBundlesUrl;
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
                        pair.Value.request.Dispose();
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

        public static void TryUnload(string mainAssetBundle)
        {
            var node = new DependencyNode(mainAssetBundle, null);
            CollectDependencies(node);
            var assetBundles = new List<DependencyNode>();
            
            while (!GetLatestDependencies(node, assetBundles))
            {
                foreach (var assetBundle in assetBundles)
                {
                    var assetBundleRef = _assetBundlesRefs[assetBundle.assetBundleName];
                    assetBundleRef.loadedCount--;
                    if (assetBundleRef.loadedCount < 0)
                        throw new Exception("loadedCount < 0, assetBundle: " + assetBundle);
                }

                assetBundles.Clear();
            }
        }

        public static bool ClearOldCachedVersions(string assetBundle)
        {
            return Caching.ClearOtherCachedVersions(assetBundle, repository[assetBundle].hash);
        }

        public static void ClearAllOldCache()
        {
            foreach (var pair in repository)
            {
                if (ClearOldCachedVersions(pair.Key));
            }
        }
        
        public static bool IsCached(string assetBundleName)
        {
            return Caching.IsVersionCached(Path.Combine(assetBundlesUrl, repository[assetBundleName].file), repository[assetBundleName].hash);
        }

        public static bool IsLoaded(string assetBundleName)
        {
            return _assetBundlesRefs.ContainsKey(assetBundleName);
        }

        public static UnityWebRequestAssetBundleFuture DownloadFromWWW(string assetBundleName, bool runImmediately = true)
        {
            if (IsCached(assetBundleName)) return null;
            Debug.Log("Download WWW: " + assetBundleName);
            var node = repository[assetBundleName];
            var f = new UnityWebRequestAssetBundleFuture(Path.Combine(assetBundlesUrl, node.file), node.hash, node.crc32, Int32.MaxValue);
            
            if (runImmediately)
            {
                f.Run();
            }
            
            return f;
        }

        private static void LoadPartial(CascadeLoading cascade, List<IProcess> processList, IEnumerable<DependencyNode> assetBundles, bool async)
        {
            foreach (var assetBundleNode in assetBundles)
            {
                string assetBundle = assetBundleNode.assetBundleName;
                
                if (_loading.ContainsKey(assetBundle))
                {
                    //loading at this moment
                    var loader = _loading[assetBundle];
                    cascade.AddFuture(loader);

                    processList?.Add(loader);
                    _assetBundlesRefs[assetBundle].loadedCount++;
                }
                else
                {
                    if (_assetBundlesRefs.ContainsKey(assetBundle) && !_loading.ContainsKey(assetBundle))
                    {
                        //already downloaded
                        _assetBundlesRefs[assetBundle].loadedCount++;
                        continue;
                    }

                    //first load
                    var node = repository[assetBundle];
                    var loader = new LoadAssetBundlePromise(node.file, assetBundlesUrl, assetBundleNode.parentNode != null, node.hash, node.crc32, async);
                    _loading.Add(assetBundle, loader);
                    _assetBundlesRefs.Add(assetBundle, new AssetBundleRef(assetBundleNode.parentNode != null));

                    loader.AddListener(future =>
                    {
                        _loading.Remove(assetBundle);
                        loadedPackedSize += repository[assetBundle].packedSize;
                        var ab = _assetBundlesRefs[assetBundle];
                        ab.allAssets = loader.GetAssets();
                        ab.assetBundle = loader.assetBundle;
                        ab.request = loader.request;
                        
                        onBundleCompleted?.Invoke(assetBundle);
                    });

                    cascade.AddFuture(loader);
                    processList?.Add(loader);
                }
            }
        }

        public static IFuture Load(string mainAssetBundleName, out List<IProcess> processList, bool async = true)
        {
            var cascade = new CascadeLoading();
            var node = new DependencyNode(mainAssetBundleName, null);
            CollectDependencies(node);
            var assetBundles = new List<DependencyNode>();
            processList = new List<IProcess>();
            
            while (!GetLatestDependencies(node, assetBundles))
            {
                LoadPartial(cascade, processList, assetBundles, async);
                assetBundles.Clear();
                cascade.Next();
            }

            cascade.Trim();
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
