using Basement.Common;
using UnityEngine;

namespace Basement.Game.AssetBundle.Repository
{
    public class AssetBundleRepositoryItem
    {
        public string[] dependencies { get; }
        public Hash128 hash { get; }
        public uint crc32 { get; }
        public int packedSize { get; }

        public AssetBundleRepositoryItem(RawNode node)
        {
            dependencies = node.GetStringArray("dependencies");
            crc32 = node.GetUInt("crc");
            packedSize = node.GetInt("size");
            hash = Hash128.Parse(node.GetString("hash"));
        }
    }
}
