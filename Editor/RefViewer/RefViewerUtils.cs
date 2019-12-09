#if REFVIEW
using System;
using System.Reflection;
using Basement.Common.Util;
using System.Collections.Generic;

namespace Editor.RefViewer
{
    public static class RefViewerUtils
    {
        public static IEnumerable<Type> FindAllDescendants(Type baseType)
        {
            foreach (var type in GetAllTypes())
            {
                Type testType = type;

                while (testType != null)
                {
                    testType = testType.BaseType;

                    if (testType == baseType)
                    {
                        yield return type;
                    }
                }
            }
        }

        public static bool IsDescendant(Type type, Type baseType)
        {
            Type testType = type;
            while (testType != null && testType != baseType)
            {
                testType = testType.BaseType;
            }

            return testType != null;
        }

        private static IEnumerable<Type> GetTypes(Assembly assembly)
        {
            var allTypes = assembly.GetTypes();

            foreach (var type in allTypes)
            {
                yield return type;
            }
        }

        private static IEnumerable<Type> GetAllTypes()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                foreach (var type in GetTypes(assembly))
                {
                    yield return type;
                }
            }
        }

        public static Type FindType(string typeName, Assembly assembly)
        {
            foreach (var type in GetTypes(assembly))
            {
                var typeText = type.ToString();
                
                if (
                    typeText.EndsWith(typeName) ||
                    typeText.EndsWith(typeName + "`1[T]") ||
                    typeText.EndsWith(typeName + "`2[T]") ||
                    typeText.EndsWith(typeName + "`3[T]") ||
                    typeText.EndsWith(typeName + "`4[T]") ||
                    typeText.EndsWith(typeName + "`5[T]")
                )
                {
                    var testType = type;

                    while (testType != null && testType != typeof(UnityEngine.Object) && testType != typeof(ReferenceCounter))
                    {
                        testType = testType.BaseType;
                    }

                    if (testType != null)
                    {
                        return type;
                    }
                }
            }

            return null;
        }

        public static Type FindType(string typeName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var type = FindType(typeName, assembly);

                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }
    }
}
#endif