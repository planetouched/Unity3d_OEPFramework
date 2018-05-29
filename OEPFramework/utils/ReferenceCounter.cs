using System;
using System.Collections.Generic;

namespace Assets.OEPFramework.utils
{
    public class ReferenceCounter
    {
        static readonly Dictionary<Type, int> references = new Dictionary<Type, int>();
        static readonly object syncRoot = new object();
        private readonly Type classType;

        public static int GetCount(Type type)
        {
            lock (syncRoot)
            {
                if (!references.ContainsKey(type)) return 0;
                return references[type];
            }
        }

        public ReferenceCounter()
        {
            lock (syncRoot)
            {
                classType = GetType();
                if (!references.ContainsKey(classType))
                {
                    references.Add(classType, 0);
                }
                references[classType]++;

                //пройдемся по base классам
                foreach (var baseType in GetBaseTypes(classType))
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
                references[classType]--;
                //пройдемся по base классам
                foreach (var baseType in GetBaseTypes(classType))
                {
                    references[baseType]--;
                }
            }
        }
    }
}
