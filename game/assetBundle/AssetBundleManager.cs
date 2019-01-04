using System;
using System.Collections.Generic;
using common;
using game.assetBundle.futures;
using game.assetBundle.repository;
using OEPFramework.futures;
using UnityEngine;
using Object = UnityEngine.Object;

namespace game.assetBundle
{
    public class AssetBundleManager
    {
        private class AssetBundleRef
        {
            public Object[] allAssets { get; set; }
            public AssetBundle assetBundle { get; set; }
            public WWW request { get; set; }
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
                        return (T)asset;
                }

                return null;
            }
            public T FindAsset<T>() where T : Object
            {
                return FindAsset<T>(_name);
            }
        }

        public int loadedPackedSize { get; private set; }

        internal class DependenceNode
        {
            public readonly DependenceNode parentNode;
            public readonly string assetBundleName;
            public readonly List<DependenceNode> nodes = new List<DependenceNode>();
            public bool stop;

            public DependenceNode(string assetBundleName, DependenceNode parent)
            {
                parentNode = parent;
                this.assetBundleName = assetBundleName;
            }
        }

        public AssetBundlesRepository repository { get; }
        public string assetBundlesUrl { get; }
        
        private readonly Dictionary<string, LoadBundlePromise> _loading = new Dictionary<string, LoadBundlePromise>();
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
                _loaded.Remove(key);

            return Resources.UnloadUnusedAssets();
        }

        public void TryUnload(string mainAssetBundle)
        {
            var node = new DependenceNode(mainAssetBundle, null);
            CollectDependencies(node);
            var assetBundles = new List<DependenceNode>();
            while (!GetLastestDependencies(node, assetBundles))
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
            return Caching.IsVersionCached(assetBundlesUrl + assetBundle, repository[assetBundle].version);
        }

        public bool IsLoaded(string assetBundle)
        {
            return _loaded.ContainsKey(assetBundle);
        }


        void LoadPartial(CascadeLoading cascase, List<IProcess> processList, IEnumerable<DependenceNode> assetBundles, bool async)
        {
            foreach (var assetBundleNode in assetBundles)
            {
                string assetBundle = assetBundleNode.assetBundleName;
                if (_loading.ContainsKey(assetBundle))
                {
                    //loading at this moment
                    var loader = _loading[assetBundle];
                    cascase.AddFuture(loader);

                    if (processList != null)
                        processList.Add(loader);
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
                    var loader = new LoadBundlePromise(assetBundle, assetBundlesUrl, assetBundleNode.parentNode != null, async, node.version, node.crc32);
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

                    cascase.AddFuture(loader);
                    if (processList != null)
                        processList.Add(loader);
                }
            }
        }

        public IFuture Load(string mainAssetBundle, out List<IProcess> processList, bool async = true)
        {
            var cascade = new CascadeLoading();
            var node = new DependenceNode(mainAssetBundle, null);
            CollectDependencies(node);
            var assetBundles = new List<DependenceNode>();
            processList = new List<IProcess>();
            while (!GetLastestDependencies(node, assetBundles))
            {
                LoadPartial(cascade, processList, assetBundles, async);
                assetBundles.Clear();
                cascade.Next();
            }

            return new CascadeLoadingPromise(cascade).Run();
        }

        bool GetLastestDependencies(DependenceNode currentNode, List<DependenceNode> list)
        {
            if (currentNode.stop)
                return true;

            if (currentNode.nodes.Count == 0)
            {
                list.Add(currentNode);

                if (currentNode.parentNode != null)
                    currentNode.parentNode.nodes.Remove(currentNode);
                else
                    currentNode.stop = true;

            }
            else
            {
                var copyCollection = new List<DependenceNode>(currentNode.nodes);
                foreach (var node in copyCollection)
                    GetLastestDependencies(node, list);
            }

            return false;
        }

        void CollectDependencies(DependenceNode parentNode)
        {
            foreach (var name in repository[parentNode.assetBundleName].dependencies)
            {
                var node = new DependenceNode(name, parentNode);
                parentNode.nodes.Add(node);
                CollectDependencies(node);
            }
        }
    }
}
