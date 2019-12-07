using System;
using System.Collections.Generic;
using System.IO;
using Basement.Common;
using Basement.OEPFramework.Futures;
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

            private readonly string _name;
            
            public AssetBundleRef(string name, bool dependency)
            {
                this.dependency = dependency;
                _name = name;
                loadedCount = 1;
            }

            public T FindAsset<T>(string assetName) where T : Object
            {
                foreach (var asset in allAssets)
                {
                    if (asset.name == assetName && asset is T)
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
        private static readonly Dictionary<string, AssetBundleRef> _loaded = new Dictionary<string, AssetBundleRef>();

        public static void Init(RawNode repositoryNode, string assetBundlesUrl)
        {
            AssetBundleManager.assetBundlesUrl = assetBundlesUrl;
            repository = new AssetBundlesRepository(repositoryNode);
        }

        public static T GetAsset<T>(string assetBundle, string assetName) where T : Object
        {
            return _loaded[assetBundle].FindAsset<T>(assetName);
        }

        public static Object[] GetAllAssets(string assetBundle)
        {
            return _loaded[assetBundle].allAssets;
        }

        public static int GetLoadedCount(string assetBundle)
        {
            return _loaded[assetBundle].loadedCount;
        }

        public static AsyncOperation UnloadUnused()
        {
            List<string> remove = new List<string>();
            foreach (var pair in _loaded)
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
                    Debug.Log("Unload: " + pair.Key);
                }
            }

            foreach (var key in remove)
            {
                _loaded.Remove(key);
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
                    var assetBundleRef = _loaded[assetBundle.assetBundleName];
                    assetBundleRef.loadedCount--;
                    if (assetBundleRef.loadedCount < 0)
                        throw new Exception("loadedCount < 0, assetBundle: " + assetBundle);
                }

                assetBundles.Clear();
            }
        }

        public static bool ClearOldCachedVersions(string assetBundle)
        {
            return Caching.ready && Caching.ClearOtherCachedVersions(assetBundle, repository[assetBundle].hash);
        }

        public static void ClearAllOldCache()
        {
            foreach (var pair in repository)
            {
                ClearOldCachedVersions(pair.Key);
            }
        }
        
        public static bool IsCached(string assetBundle)
        {
            return Caching.ready && Caching.IsVersionCached(Path.Combine(assetBundlesUrl, assetBundle), repository[assetBundle].hash);
        }

        public static bool IsLoaded(string assetBundle)
        {
            return _loaded.ContainsKey(assetBundle);
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
                    _loaded[assetBundle].loadedCount++;
                }
                else
                {
                    if (_loaded.ContainsKey(assetBundle) && !_loading.ContainsKey(assetBundle))
                    {
                        //already downloaded
                        _loaded[assetBundle].loadedCount++;
                        continue;
                    }

                    //first load
                    var node = repository[assetBundle];
                    var loader = new LoadAssetBundlePromise(node.file, assetBundlesUrl, assetBundleNode.parentNode != null, node.hash, node.crc32, async);
                    _loading.Add(assetBundle, loader);
                    _loaded.Add(assetBundle, new AssetBundleRef(assetBundle, assetBundleNode.parentNode != null));
                    string bundle = assetBundle;

                    loader.AddListener(future =>
                    {
                        loadedPackedSize += repository[bundle].packedSize;
                        _loading.Remove(bundle);
                        var ab = _loaded[bundle];
                        ab.allAssets = loader.GetAssets();
                        ab.assetBundle = loader.assetBundle;
                        ab.request = loader.request;
                    });

                    cascade.AddFuture(loader);
                    processList?.Add(loader);
                }
            }
        }

        public static IFuture Load(string mainAssetBundle, out List<IProcess> processList, bool async = true)
        {
            var cascade = new CascadeLoading();
            var node = new DependencyNode(mainAssetBundle, null);
            CollectDependencies(node);
            var assetBundles = new List<DependencyNode>();
            processList = new List<IProcess>();
            
            while (!GetLatestDependencies(node, assetBundles))
            {
                LoadPartial(cascade, processList, assetBundles, async);
                assetBundles.Clear();
                cascade.Next();
            }

            return new CascadeLoadingPromise(cascade).Run();
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
