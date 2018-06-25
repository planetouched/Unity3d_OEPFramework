using System;
using System.Collections.Generic;

namespace Assets.common.service
{
    public static class ServiceManager
    {
        static readonly Dictionary<Type, IService> services = new Dictionary<Type, IService>();

        public static IService Add(IService service)
        {
            services.Add(service.GetType(), service);
            return service;
        }

        public static T Get<T>() where T : class, IService
        {
            return (T)services[typeof(T)];
        }

        public static void Remove<T>() where T : class, IService
        {
            var service = Get<T>();
            if (service != null)
            {
                services.Remove(typeof (T));
            }
        }

        public static void RemoveAll()
        {
            services.Clear();
        }
    }
}
