using System;
using System.Collections.Generic;
using Basement.Common;
using Basement.OEPFramework.Futures;
using Game.AssetBundle.Futures;
using Game.AssetBundle.Repository;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace Game.AssetBundle
{
    public class AssetBundleManager
    {
        private class AssetBundleRef
        {
            public Object[] allAssets { get; set; }
            public UnityEngine.AssetBundle assetBundle { get; set; }
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
            public T FindAsset<T>() where T : Object
            {
                return FindAsset<T>(_name);
            }
        }

        internal class DependencyNode
        {
            public DependencyNode parentNode { get; }
            public string assetBundleName { get; }
            public List<DependencyNode> nodes { get; set; } = new List<DependencyNode>();
            public bool stop { get; set; }

            public DependencyNode(string assetBundleName, DependencyNode parent)
            {
                parentNode = parent;
                this.assetBundleName = assetBundleName;
            }
        }

        public int loadedPackedSize { get; private set; }
        public AssetBundlesRepository repository { get; }
        public string assetBundlesUrl { get; }
        
        private readonly Dictionary<string, LoadAssetBundlePromise> _loading = new Dictionary<string, LoadAssetBundlePromise>();
        private readonly Dictionary<string, AssetBundleRef> _loaded = new Dictionary<string, AssetBundleRef>();

        public AssetBundleManager(RawNode repositoryNode, string assetBundlesUrl)
        {
            this.assetBundlesUrl = assetBundlesUrl;
            repository = new AssetBundlesRepository(repositoryNode);
        }

        public T GetAsset<T>(string assetBundle) where T : Object
        {
            return _loaded[assetBundle].FindAsset<T>();
        }

        public T GetAsset<T>(string assetBundle, string assetName) where T : Object
        {
            return _loaded[assetBundle].FindAsset<T>(assetName);
        }

        public int GetLoadedCount(string assetBundle)
        {
            return _loaded[assetBundle].loadedCount;
        }

        public AsyncOperation UnloadUnused()
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

        public void TryUnload(string mainAssetBundle)
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

        public bool IsCached(string assetBundle)
        {
            return Caching.IsVersionCached(assetBundlesUrl + assetBundle, repository[assetBundle].hash);
        }

        public bool IsLoaded(string assetBundle)
        {
            return _loaded.ContainsKey(assetBundle);
        }


        private void LoadPartial(CascadeLoading cascade, List<IProcess> processList, IEnumerable<DependencyNode> assetBundles, bool async)
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
                    var loader = new LoadAssetBundlePromise(assetBundle, assetBundlesUrl, assetBundleNode.parentNode != null, node.hash, node.crc32, async);
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

        public IFuture Load(string mainAssetBundle, out List<IProcess> processList, bool async = true)
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

        private bool GetLatestDependencies(DependencyNode currentNode, List<DependencyNode> list)
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

        private void CollectDependencies(DependencyNode parentNode)
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
