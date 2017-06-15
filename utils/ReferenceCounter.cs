using System;
using System.Collections.Generic;

namespace OEPFramework.utils
{
    public class ReferenceCounter
    {
        static readonly Dictionary<Type, int> references = new Dictionary<Type, int>();
        static readonly object syncRoot = new object();
        public static int GetCount(Type type)
        {
            lock (syncRoot)
            {
                if (!references.ContainsKey(type)) return 0;
                return references[type];
            }
        }

        private readonly Type _type;
        public ReferenceCounter()
        {
            lock (syncRoot)
            {
                _type = GetType();
                if (!references.ContainsKey(_type))
                {
                    references.Add(_type, 0);
                }
                references[_type]++;

                //пройдемся по base классам
                foreach (var baseType in GetBaseTypes(_type))
                {
                    if (!references.ContainsKey(baseType))
                        references.Add(baseType, 0);

                    references[baseType]++;
                }
            }
        }

        IEnumerable<Type> GetBaseTypes(Type type)
        {
            Type baseType = type.BaseType;
            while (baseType != null)
            {
                yield return baseType;
                baseType = baseType.BaseType;
            }
            
        }

        ~ReferenceCounter()
        {
            lock (syncRoot)
            {
                references[_type]--;
                //пройдемся по base классам
                foreach (var baseType in GetBaseTypes(_type))
                {
                    references[baseType]--;
                }
            }
        }
    }
}
