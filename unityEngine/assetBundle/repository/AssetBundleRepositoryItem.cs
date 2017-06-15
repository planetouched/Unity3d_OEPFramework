using OEPFramework.common;

namespace OEPFramework.unityEngine.assetBundle.repository
{
    public class AssetBundleRepositoryItem
    {
        public string[] dependencies { get; private set; }
        public int version { get; private set; }
        public AssetBundleRepositoryItem(RawNode node)
        {
            dependencies = node.GetStringArray("dependencies");
            version = node.GetInt("version");
        }
    }
}
