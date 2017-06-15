using System;
using System.Collections.Generic;
using OEPFramework.common;
using OEPFramework.common.future;
using OEPFramework.unityEngine.assetBundle.future;
using OEPFramework.unityEngine.assetBundle.repository;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OEPFramework.unityEngine.assetBundle
{
    public class AssetBundleManager
    {
        internal class AssetBundleRef
        {
            public Object[] allAssets { get; set; }
            public AssetBundle assetBundle { get; set; }
            public WWW request { get; set; }
            public int loadedCount { get; set; }
            private readonly string name;
            public bool dependency { get; private set; }

            public AssetBundleRef(string name, bool dependency)
            {
                this.dependency = dependency;
                this.name = name;
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
                return FindAsset<T>(name);
            }
        }

        internal class DependenceNode
        {
            public DependenceNode parentNode;
            public string assetBundleName;
            public readonly List<DependenceNode> nodes = new List<DependenceNode>();
            public bool stop;

            public DependenceNode(string assetBundleName, DependenceNode parent)
            {
                parentNode = parent;
                this.assetBundleName = assetBundleName;
            }
        }

        public AssetBundlesRepository Repository
        {
            get { return repository; }
        }

        private readonly Dictionary<string, LoadBundlePromise> loading = new Dictionary<string, LoadBundlePromise>();
        private readonly Dictionary<string, AssetBundleRef> loaded = new Dictionary<string, AssetBundleRef>();
        private readonly AssetBundlesRepository repository;
        private readonly string assetBundlesUrl;

        public AssetBundleManager(RawNode repository, string assetBundlesUrl)
        {
            this.assetBundlesUrl = assetBundlesUrl;
            this.repository = new AssetBundlesRepository(repository);
        }

        public T GetAsset<T>(string assetBundle) where T : Object
        {
            return loaded[assetBundle].FindAsset<T>();
        }

        public T GetAsset<T>(string assetBundle, string assetName) where T : Object
        {
            return loaded[assetBundle].FindAsset<T>(assetName);
        }

        public int GetLoadedCount(string assetBundle)
        {
            return loaded[assetBundle].loadedCount;
        }

        public AsyncOperation UnloadUnused()
        {
            List<string> remove = new List<string>();
            foreach (var pair in loaded)
            {
                if (pair.Value.loadedCount == 0)
                {
                    pair.Value.allAssets = null;
                    if (pair.Value.dependency)
                    {
                        pair.Value.assetBundle.Unload(true);
                        pair.Value.request.Dispose();
                    }

                    remove.Add(pair.Key);
                    Debug.Log("Unload: " + pair.Key);
                }
            }

            foreach (var key in remove)
                loaded.Remove(key);

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
                    var assetBundleRef = loaded[assetBundle.assetBundleName];
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
            return loaded.ContainsKey(assetBundle);
        }


        void LoadPartial(CascadeLoading cascase, List<IProcess> processList, IEnumerable<DependenceNode> assetBundles, bool async)
        {
            foreach (var assetBundleNode in assetBundles)
            {
                string assetBundle = assetBundleNode.assetBundleName;
                if (loading.ContainsKey(assetBundle))
                {
                    //loading at this moment
                    var loader = loading[assetBundle];
                    cascase.AddFuture(loader);

                    if (processList != null)
                        processList.Add(loader);
                    loaded[assetBundle].loadedCount++;
                }
                else
                {
                    if (loaded.ContainsKey(assetBundle) && !loading.ContainsKey(assetBundle))
                    {
                        //already downloaded
                        loaded[assetBundle].loadedCount++;
                        continue;
                    }

                    //first load
                    var loader = new LoadBundlePromise(assetBundle, assetBundlesUrl, assetBundleNode.parentNode != null, repository[assetBundle].version, async);
                    loading.Add(assetBundle, loader);
                    loaded.Add(assetBundle, new AssetBundleRef(assetBundle, assetBundleNode.parentNode != null));
                    string bundle = assetBundle;

                    loader.AddListener(future =>
                    {
                        loading.Remove(bundle);
                        var ab = loaded[bundle];
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
