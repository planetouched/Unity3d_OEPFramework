using Basement.Common;
using UnityEngine;

namespace OEPCommon.AssetBundles.Repository
{
    public class AssetBundleRepositoryItem
    {
        public string[] dependencies { get; }
        public string hash { get; }
        public uint crc32 { get; }
        public int packedSize { get; }
        public bool isDependency { get; }
        
        public string file { get; }

        public AssetBundleRepositoryItem(RawNode node)
        {
            dependencies = node.GetStringArray("dependencies");
            crc32 = node.GetUInt("crc");
            packedSize = node.GetInt("size");
            hash = node.GetString("hash");
            file = node.GetString("file");
            isDependency = node.GetBool("dependency");
        }
    }
}
