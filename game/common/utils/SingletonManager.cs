using System;
using System.Collections.Generic;

namespace Assets.game.common.utils
{
    public static class SingletonManager
    {
        static readonly Dictionary<Type, object> singletones = new Dictionary<Type, object>();

        public static T Add<T>(T singleton)
        {
            singletones.Add(typeof(T), singleton);
            return singleton;
        }

        public static T Get<T>()
        {
            return (T)singletones[typeof (T)];
        }

        public static T Remove<T>()
        {
            var obj = Get<T>();
            singletones.Remove(typeof (T));
            return obj;
        }

        public static List<Tuple<Type, object>> RemoveAll()
        {
            var list = new List<Tuple<Type, object>>();

            foreach (var pair in singletones)
                list.Add(new Tuple<Type, object>(pair.Key, pair.Value));

            singletones.Clear();
            return list;
        }
    }
}
