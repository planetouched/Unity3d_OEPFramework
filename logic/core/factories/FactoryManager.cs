using System;
using System.Collections.Generic;
using Assets.common;
using Assets.logic.core.context;

namespace Assets.logic.core.factories
{
    public static class FactoryManager
    {
        private static readonly Dictionary<Type, IFactory> factories = new Dictionary<Type, IFactory>();

        private static IDefaultFactory defaultFactory;

        public static void SetDefaultFactory(IDefaultFactory factory)
        {
            defaultFactory = factory;
        }

        public static IFactory AddFactory(Type factoryType, IFactory factory)
        {
            factories.Add(factoryType, factory);
            return factory;
        }

        public static IFactory GetFactory(Type factoryType)
        {
            return factories[factoryType];
        }
        
        #region common
        public static object Build(Type type, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                IFactory factory;
                if (factories.TryGetValue(type, out factory))
                    return factory.Build(key);
            }
            return defaultFactory.Build(type);
        }

        public static object Build<T1>(Type type, string key, T1 param1)
        {
            if (!string.IsNullOrEmpty(key))
            {
                IFactory factory;
                if (factories.TryGetValue(type, out factory))
                    return factory.Build(key, param1);
            }
            return defaultFactory.Build(type, param1);
        }

        public static object Build<T1, T2>(Type type, string key, T1 param1, T2 param2)
        {
            if (!string.IsNullOrEmpty(key))
            {
                IFactory factory;
                if (factories.TryGetValue(type, out factory))
                    return factory.Build(key, param1, param2);
            }
            return defaultFactory.Build(type, param1, param2);
        }

        public static object Build<T1, T2, T3>(Type type, string key, T1 param1, T2 param2, T3 param3)
        {
            if (!string.IsNullOrEmpty(key))
            {
                IFactory factory;
                if (factories.TryGetValue(type, out factory))
                    return factory.Build(key, param1, param2, param3);
            }
            return defaultFactory.Build(type, param1, param2, param3);
        }

        public static object Build<T1, T2, T3, T4>(Type type, string key, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            if (!string.IsNullOrEmpty(key))
            {
                IFactory factory;
                if (factories.TryGetValue(type, out factory))
                    return factory.Build(key, param1, param2, param3, param4);
            }
            return defaultFactory.Build(type, param1, param2, param3, param4);
        }

        public static object Build<T1, T2, T3, T4, T5>(Type type, string key, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
        {
            if (!string.IsNullOrEmpty(key))
            {
                IFactory factory;
                if (factories.TryGetValue(type, out factory))
                    return factory.Build(key, param1, param2, param3, param4, param5);
            }
            return defaultFactory.Build(type, param1, param2, param3, param4, param5);
        }

        #endregion

        #region build by node
        
        public static TRet Build<TRet>(string key, RawNode rawNode)
        {
            Type type = typeof(TRet);
            IFactory factory;
            if (factories.TryGetValue(type, out factory))
                return (TRet)factory.Build(key, rawNode);

            return (TRet)defaultFactory.Build(type, rawNode);
        }

        public static TRet Build<TRet>(string key, RawNode rawNode, IContext context)
        {
            Type type = typeof(TRet);
            IFactory factory;
            if (factories.TryGetValue(type, out factory))
                return (TRet)factory.Build(key, rawNode, context);

            return (TRet)defaultFactory.Build(type, rawNode, context);
        }
      
        public static TRet Build<TRet>(RawNode rawNode)
        {
            Type baseType = typeof(TRet);
            if (rawNode.CheckKey("type"))
                return Build<TRet>(rawNode.GetString("type"), rawNode);

            return (TRet)defaultFactory.Build(baseType, rawNode);
        }

        public static TRet Build<TRet>(RawNode rawNode, IContext context)
        {
            Type baseType = typeof(TRet);
            if (rawNode.CheckKey("type"))
                return Build<TRet>(rawNode.GetString("type"), rawNode, context);

            return (TRet)defaultFactory.Build(baseType, rawNode, context);
        }

        #endregion
    }
}
