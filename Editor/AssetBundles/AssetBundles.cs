using System.Collections.Generic;
using System.IO;
using fastJSON;
using UnityEditor;
using UnityEngine;

namespace Editor.AssetBundles
{
    public static class CreateAssetBundles
    {
        [MenuItem("DevTools/Build Tools/AssetBundles (Windows, Editor)")]
        public static void BuildWindowsAssetBundles()
        {
            Debug.Log("AssetBundles target Windows");
            Wipe("Windows");
            var manifest = BuildPipeline.BuildAssetBundles("AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
            CompileManifest("Windows", manifest);
        }

        [MenuItem("DevTools/Build Tools/AssetBundles (Android)")]
        public static void BuildAndroidAssetBundles()
        {
            Debug.Log("AssetBundles target Android");
            Wipe("Android");
            var manifest = BuildPipeline.BuildAssetBundles("AssetBundles", BuildAssetBundleOptions.None, BuildTarget.Android);
            CompileManifest("Android", manifest);
        }

        [MenuItem("DevTools/Build Tools/AssetBundles (iOS)")]
        public static void BuildIPhoneAssetBundles()
        {
            Debug.Log("AssetBundles target Android");
            Wipe("IOS");
            var manifest = BuildPipeline.BuildAssetBundles("AssetBundles", BuildAssetBundleOptions.None, BuildTarget.iOS);
            CompileManifest("IOS", manifest);
        }

        private static void Wipe(string directory)
        {
            var platformDirectory = Application.dataPath + "/../AssetBundles/" + directory;
            if (Directory.Exists(platformDirectory))
            {
                Directory.Delete(platformDirectory, true);
            }
        }

        private static void CompileManifest(string directory, AssetBundleManifest manifest)
        {
            var platformDirectory = Application.dataPath + "/../AssetBundles/" + directory;
            Directory.CreateDirectory(platformDirectory);

            string repoFile = Application.dataPath + "/../AssetBundles/asset-bundles.json";

            var items = new Dictionary<string, Dictionary<string, object>>();

            var exist = new HashSet<string>();

            foreach (var item in manifest.GetAllAssetBundles())
            {
                exist.Add(item);

                if (!items.ContainsKey(item))
                {
                    var file = Application.dataPath + "/../AssetBundles/" + item;
                    BuildPipeline.GetCRCForAssetBundle(file, out var crc32);
                    BuildPipeline.GetHashForAssetBundle(file, out var hash128);
                    var fi = new FileInfo(file);

                    items.Add(item, new Dictionary<string, object>());
                    var node = items[item];

                    node.Add("crc", crc32);
                    node.Add("hash", hash128.ToString());
                    node.Add("file", item + "_" + hash128 + "/" + item);
                    node.Add("dependencies", manifest.GetDirectDependencies(item));
                    node.Add("size", fi.Length);

                    Directory.CreateDirectory(platformDirectory + "/" + item + "_" + hash128);
                    fi.CopyTo(platformDirectory + "/" + item + "_" + hash128 + "/" + item, true);
                }
                else
                {
                    items[item]["dependencies"] = manifest.GetDirectDependencies(item);
                }
            }

            var removeList = new List<string>();

            foreach (var item in items.Keys)
            {
                if (!exist.Contains(item))
                {
                    removeList.Add(item);
                }
            }

            foreach (var item in removeList)
            {
                items.Remove(item);
            }

            File.WriteAllText(repoFile, JSON.Instance.ToJSON(items));
        }
    }
}