using Assets.framework.core;
using UnityEngine;

namespace OEPFramework.unityEngine.assetBundle.repository
{
    public class AssetBundleRepositoryItem
    {
        public string[] dependencies { get; private set; }
        public Hash128 version { get; private set; }
        public uint crc32 { get; private set; }

        public AssetBundleRepositoryItem(RawNode node)
        {
            dependencies = node.GetStringArray("dependencies");
            crc32 = node.GetUInt("crc");
            version = Hash128.Parse(node.GetString("hash"));
        }
    }
}
