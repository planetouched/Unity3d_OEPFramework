using UnityEngine;

namespace OEPFramework.unityEngine.extension
{
    public static class Extensions
    {
        public static T GetCachedComponents<T>(this GameObject go) where T : CachedComponents, new()
        {
            var components = new T();
            components.SetGameObject(go);
            return components;
        }

        public static CachedComponents GetCachedComponents(this GameObject go)
        {
            var components = new CachedComponents();
            components.SetGameObject(go);
            return components;
        }
		
		public static void SetLayerRecursive(this GameObject go, int layer)
        {
            go.layer = layer;
            int count = go.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform t = go.transform.GetChild(i);
                t.gameObject.layer = layer;
                if (t.childCount > 0)
                    SetLayerRecursive(t.gameObject, layer);
            }
        }
    }
}