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
    }
}