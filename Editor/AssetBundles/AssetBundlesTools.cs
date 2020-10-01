using System.Collections.Generic;
using System.IO;
using fastJSON;
using OEPCommon.AssetBundles;
using UnityEditor;
using UnityEngine;

namespace Editor.GameTools
{
    public static class AssetBundlesTools
    {
        [MenuItem("DevTools/Asset Bundles Tools/AssetBundles (Windows, Editor)")]
        public static void BuildWindowsAssetBundles()
        {
            Debug.Log("AssetBundles target Windows");
            string platform = RuntimePlatform.WindowsPlayer.ToString();
            Wipe(platform);
            var manifest = BuildPipeline.BuildAssetBundles("AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
            CompileManifest(platform, manifest);
        }

        [MenuItem("DevTools/Asset Bundles Tools/AssetBundles (Android)")]
        public static void BuildAndroidAssetBundles()
        {
            Debug.Log("AssetBundles target Android");
            string platform = RuntimePlatform.Android.ToString();
            Wipe(platform);
            var manifest = BuildPipeline.BuildAssetBundles("AssetBundles", BuildAssetBundleOptions.None, BuildTarget.Android);
            CompileManifest(platform, manifest);
        }

        [MenuItem("DevTools/Asset Bundles Tools/AssetBundles (iOS)")]
        public static void BuildIPhoneAssetBundles()
        {
            Debug.Log("AssetBundles target IOS");
            string platform = RuntimePlatform.IPhonePlayer.ToString();
            Wipe(platform);
            var manifest = BuildPipeline.BuildAssetBundles("AssetBundles", BuildAssetBundleOptions.None, BuildTarget.iOS);
            CompileManifest(platform, manifest);
        }

        private static void Wipe(string directory)
        {
            var platformDirectory = Application.dataPath.Replace("/Assets", string.Empty) + "/AssetBundles/" + directory;

            if (Directory.Exists(platformDirectory))
            {
                Directory.Delete(platformDirectory, true);
            }

            Directory.CreateDirectory(platformDirectory);
        }

        private static void CompileManifest(string directory, AssetBundleManifest manifest)
        {
            var assetBundleCrc32 = new Crc32();
            
            var projectPath = Application.dataPath.Replace("/Assets", string.Empty);
            var platformDirectory = projectPath + "/AssetBundles/" + directory;

            var repoFile = projectPath + "/AssetBundles/asset-bundles.json";

            var items = new Dictionary<string, Dictionary<string, object>>();
            var exist = new HashSet<string>();
            var dependencies = new HashSet<string>();

            foreach (var item in manifest.GetAllAssetBundles())
            {
                exist.Add(item);

                if (!items.ContainsKey(item))
                {
                    var file = projectPath + "/AssetBundles/" + item;

                    //BuildPipeline.GetCRCForAssetBundle(file, out var crc32);
                    BuildPipeline.GetHashForAssetBundle(file, out var hash128);

                    var fileInfo = new FileInfo(file);

                    items.Add(item, new Dictionary<string, object>());

                    var node = items[item];

                    node.Add("crc", assetBundleCrc32.ComputeChecksum(File.ReadAllBytes(file)));
                    node.Add("hash", hash128.ToString());
                    node.Add("file", item + "_" + hash128 + "/" + item);
                    node.Add("dependencies", manifest.GetDirectDependencies(item));

                    foreach (var dep in manifest.GetDirectDependencies(item))
                    {
                        if (!dependencies.Contains(dep))
                        {
                            dependencies.Add(dep);
                        }
                    }
                    
                    node.Add("size", fileInfo.Length);

                    Directory.CreateDirectory(platformDirectory + "/" + item + "_" + hash128);

                    fileInfo.CopyTo(platformDirectory + "/" + item + "_" + hash128 + "/" + item, true);
                    fileInfo.Delete();
                }
                else
                {
                    items[item]["dependencies"] = manifest.GetDirectDependencies(item);
                    
                    foreach (var dep in manifest.GetDirectDependencies(item))
                    {
                        if (!dependencies.Contains(dep))
                        {
                            dependencies.Add(dep);
                        }
                    }                    
                }
            }

            foreach (var dep in dependencies)
            {
                items[dep]["dependency"] = true;
            }
            
            var manifests = Directory.GetFiles(projectPath + "/AssetBundles/", "*.manifest", SearchOption.AllDirectories);

            foreach (var file in manifests)
            {
                File.Delete(file);
            }

            File.Delete(projectPath + "/AssetBundles/AssetBundles");

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

            var toFile = projectPath + $"/_web/{directory.ToLower()}-asset-bundles.json";
           
            
            if (! Directory.Exists(projectPath + $"/_web/"))
            {
                Directory.CreateDirectory(projectPath + $"/_web/");
            }
            
            if (File.Exists(toFile))
            {
                File.Delete(toFile);
            }

            File.Move(repoFile, toFile);
        }
    }
}