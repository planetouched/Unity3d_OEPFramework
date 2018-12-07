using System.Collections.Generic;
using Assets.logic.core.model;

namespace Assets.logic.core.util
{
    public static class SerializeUtil
    {
        public static Dictionary<string, object> SetArgs(this Dictionary<string, object> dict, params object[] args)
        {
            for (int i = 0; i < args.Length / 2; i++)
            {
                dict[(string)args[i * 2]] = args[i * 2 + 1];
            }

            return dict;
        }

        public static Dictionary<string, object> Dict(object fromDictionary = null)
        {
            if (fromDictionary != null)
            {
                return (Dictionary<string, object>) fromDictionary;
            }

            return new Dictionary<string, object>();
        }

        /*
        public static Dictionary<string, object> Dict(params object[] args)
        {
            var dict = new Dictionary<string, object>();

            for (int i = 0; i < args.Length / 2; i++)
            {
                dict[(string)args[i * 2]] = args[i * 2 + 1];
            }

            return dict;
        }*/
        
        public static IList<object> SerializeArray<T>(IEnumerable<T> collection) where T : ISerialize
        {
            var list = new List<object>();

            foreach (var item in collection)
            {
                list.Add(item.Serialize());
            }

            return list;
        }
    }
}
