using System;
using System.Collections.Generic;

namespace OEPFramework.common
{
    public class RawNode
    {
        public static RawNode emptyNode = new RawNode();
        static readonly List<object> emptyList = new List<object>();
        
        public string id { get; private set; }
        private readonly object rawData;
        public int nodesCount { get { return dictionary.Count; } }
        private KeyValuePair<string, RawNode>[] _sortedCache;
        private KeyValuePair<string, RawNode>[] _unsortedCache;

        public RawNode(object rawData)
        {
            this.rawData = rawData;
        }

        private RawNode()
        {
            
        }

        public RawNode(params object [] args)
        {
            if (args == null || args.Length == 0 || args.Length % 2 == 1)
                throw new Exception("CreateParams Invalid params");

            var tmp = new Dictionary<string, object>();
            for (int i = 0; i < args.Length; i += 2)
            {
                tmp.Add((string)args[i], args[i + 1]);
            }
            rawData = tmp;
        }

        public object GetRawData()
        {
            return rawData;
        }


        #region setNode

        public void SetValue<T>(string key, T value)
        {
            dictionary[key] = value;
        }

        #endregion

        #region getNode
        public int GetInt(string key, int defaultValue = 0)
        {
            object value;
            if (rawData != null && dictionary.TryGetValue(key, out value))
                return Convert.ToInt32(value);
            return defaultValue;
        }

        public uint GetUInt(string key, uint defaultValue = 0)
        {
            object value;
            if (rawData != null && dictionary.TryGetValue(key, out value))
                return Convert.ToUInt32(value);
            return defaultValue;
        }

        public long GetLong(string key, long defaultValue = 0)
        {
            object value;
            if (rawData != null && dictionary.TryGetValue(key, out value))
                return (long)value;
            return defaultValue;
        }

        public float GetFloat(string key, float defaultValue = 0)
        {
            object value;
            if (rawData != null && dictionary.TryGetValue(key, out value))
                return Convert.ToSingle(value);
            return defaultValue;
        }

        public double GetDouble(string key, double defaultValue = 0)
        {
            object value;
            if (rawData != null && dictionary.TryGetValue(key, out value))
                return Convert.ToDouble(value);
            return defaultValue;
        }

        public string GetString(string key, string defaultValue = "")
        {
            object value;
            if (rawData != null && dictionary.TryGetValue(key, out value))
                //return (string)value;
                return value.ToString();
            return defaultValue;
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            object value;
            if (rawData != null && dictionary.TryGetValue(key, out value))
                //return (bool)value;
                return Convert.ToBoolean(value);
            return defaultValue;
        }

        #endregion
        #region getSelf

        public int ToInt()
        {
            return rawData != null ? Convert.ToInt32(rawData) : 0;
        }

        public long ToLong()
        {
            return rawData != null ? (long)rawData : 0;
        }
        
        public float ToFloat()
        {
            return rawData != null ? Convert.ToSingle(rawData) : 0;
        }
        
        public double ToDouble()
        {
            return rawData != null ? Convert.ToDouble(rawData) : 0;
        }

        public override string ToString()
        {
            return rawData != null ? rawData.ToString() : "";
        }
        #endregion
        #region getArray

        public int[] GetIntArray(string key)
        {
            object value = null;
            if (rawData != null)
                dictionary.TryGetValue(key, out value);
            return new RawNode(value).GetIntArray();
        }

        public int[] GetIntArray()
        {
            if (rawData == null) return new int[0];
            var ret = new List<int>();
            foreach (var e in (List<object>)rawData)
                ret.Add(Convert.ToInt32(e));
            return ret.ToArray();
        }

        public float[] GetFloatArray(string key)
        {
            object value = null;
            if (rawData != null)
                dictionary.TryGetValue(key, out value);

            return new RawNode(value).GetFloatArray();
        }

        public float[] GetFloatArray()
        {
            if (rawData == null) return new float[0];
            var ret = new List<float>();
            foreach (var e in (List<object>)rawData)
                ret.Add(Convert.ToSingle(e));
            return ret.ToArray();
        }

        public double[] GetDoubleArray(string key)
        {
            object value = null;
            if (rawData != null)
                dictionary.TryGetValue(key, out value);

            return new RawNode(value).GetDoubleArray();
        }

        public double[] GetDoubleArray()
        {
            if (rawData == null) return new double[0];
            var ret = new List<double>();
            foreach (var e in (List<object>)rawData)
                ret.Add(Convert.ToDouble(e));
            return ret.ToArray();
        }

        public string[] GetStringArray(string key)
        {
            object value = null;
            if (rawData != null)
                dictionary.TryGetValue(key, out value);

            return new RawNode(value).GetStringArray();
        }

        public string[] GetStringArray()
        {
            if (rawData == null) return new string[0];
            var ret = new List<string>();
            foreach (var e in (List<object>)rawData)
                ret.Add(Convert.ToString(e));
            return ret.ToArray();
        }

        public List<Dictionary<string, object>> GetObjectArray(string key)
        {
            object value = null;
            if (rawData != null)
                dictionary.TryGetValue(key, out value);

            return new RawNode(value).GetObjectArray();
        }

        public List<Dictionary<string, object>> GetObjectArray()
        {
            if (rawData == null) return new List<Dictionary<string, object>>();
            var ret = new List<Dictionary<string, object>>();
            foreach (var e in (List<object>)rawData)
                ret.Add((Dictionary<string, object>)e);
            return ret;
        }

        #endregion


        public IEnumerable<KeyValuePair<string, RawNode>> GetSortedCollection()
        {
            if (dictionary != null)
            {
                if (_sortedCache == null)
                {
                    var keys = new string[dictionary.Count];
                    _sortedCache = new KeyValuePair<string, RawNode>[keys.Length];
                    dictionary.Keys.CopyTo(keys, 0);
                    Array.Sort(keys, StringComparer.InvariantCulture);

                    for (int i = 0; i < keys.Length; i++)
                    {
                        var key = keys[i];
                        _sortedCache[i] = new KeyValuePair<string, RawNode>(key, new RawNode(dictionary[key]));
                    }
                }

                foreach (var pair in _sortedCache)
                    yield return pair;
            }
        }

        public IEnumerable<KeyValuePair<string, RawNode>> GetUnsortedCollection()
        {
            if (dictionary != null)
            {
                if (_unsortedCache == null)
                {
                    _unsortedCache = new KeyValuePair<string, RawNode>[dictionary.Count];
                    int idx = 0;
                    foreach (var pair in dictionary)
                        _unsortedCache[idx++] = new KeyValuePair<string, RawNode>(pair.Key, new RawNode(pair.Value));
                }

                foreach (var pair in _unsortedCache)
                    yield return pair;
            }
        }
        
        public RawNode GetNode(string key, char separator = '/')
        {
            //ключ может быть путем
            if (rawData != null)
            {
                string[] path = key.Split(separator);
                object value;
                if (dictionary.TryGetValue(path[0], out value))
                {
                    var node = new RawNode(value) { id = path[0] };
                    for (int i = 1; i < path.Length; i++)
                    {
                        if (node.dictionary.ContainsKey(path[i]))
                        {
                            node = node.GetNode(path[i]);
                            node.id = path[i];
                        }
                        else
                        {
                            return emptyNode;
                        }
                    }
                    return node;
                }
            }
            return new RawNode { id = key };
        }

        public bool IsInit()
        {
            return rawData != null;
        }
        
        public bool CheckKey(string key)
        {
            return dictionary != null && dictionary.ContainsKey(key);
        }

        public RawNode GetNode(int index)
        {
            return rawData != null ? new RawNode(array[index]) : emptyNode;
        }

        public List<object> array
        {
            get
            {
                if (rawData == null)
                    return emptyList;
                return (List<object>) rawData;
            }
        }

        public Dictionary<string, object> dictionary 
        {
            get
            {
                if (rawData == null)
                    return new Dictionary<string, object>();
                return (Dictionary<string, object>) rawData;
            }
        }

        public RawNode Concatenate(RawNode node)
        {
            if (node == null)
                return this;

            var toAddDictionary = dictionary;
            var addDictionary = node.dictionary;
            var keys = addDictionary.Keys;
            foreach (var k in keys)
            {
                if (!toAddDictionary.ContainsKey(k))
                    toAddDictionary.Add(k, addDictionary[k]);
            }
            return new RawNode(toAddDictionary);            
        }
    }
}
