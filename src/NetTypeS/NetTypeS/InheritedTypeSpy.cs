using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetTypeS.Interfaces;
using NetTypeS.Utils;

namespace NetTypeS
{
    internal class InheritedTypeSpy : IInheritedTypeSpy
    {
        private readonly Lazy<IDictionary<Type, Type[]>> _typeInheritance;

        public InheritedTypeSpy(params Assembly[] assemblies)
        {
            _typeInheritance = new Lazy<IDictionary<Type, Type[]>>(() => InitializeTypes(assemblies));
        }

        private static void AddChildType(Type child, Type parent, IDictionary<Type, List<Type>> typeDictionary)
        {
            List<Type> childTypes;
            if (!typeDictionary.TryGetValue(parent, out childTypes))
            {
                childTypes = new List<Type>();
                typeDictionary.Add(parent, childTypes);
            }
            childTypes.Add(child);
        }

        private static void PopulateTypeParents(Type type, IDictionary<Type, List<Type>> typeDictionary)
        {
            var interfaces = type.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                AddChildType(type, @interface, typeDictionary);
            }
            var baseType = type.BaseType;
            if (baseType != null && baseType != typeof(object))
                AddChildType(type, baseType, typeDictionary);
        }

        private static IDictionary<Type, Type[]> InitializeTypes(Assembly[] assemblies)
        {
            var result = new Dictionary<Type, List<Type>>();
            var loadedAssemblies = assemblies == null || assemblies.Length == 0 ? AppDomain.CurrentDomain.GetAssemblies() : assemblies;
            foreach (var assembly in loadedAssemblies)
            {
                Type[] assemblyTypes = new Type[0];
                try
                {
                    assemblyTypes = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException)
                {
                    // Ignore assemblies for which we unable to load some types.
                    // This happens often, as types are loaded lazily, and application itself doesn't require all types in used assemblies.
                }

                foreach (var type in assemblyTypes)
                {
                    // we skip from processing some types
                    if (type.IsEnum || type.IsGenericParameter || type.IsArray || type.IsConstructedGenericType ||
                        type.IsPointer || type.IsPrimitive || (type.IsAbstract && type.IsSealed /*static*/) ||
                        type == typeof(object) || type.IsCollection())
                        continue;
                    PopulateTypeParents(type, result);
                }
            }

            return result.ToDictionary(kv => kv.Key, kv => kv.Value.ToArray());
        }

        public IEnumerable<Type> GetInheritedTypes(Type type)
        {
            Type[] types;
            return type == null || !_typeInheritance.Value.TryGetValue(type, out types) ? Enumerable.Empty<Type>() : types;
        }
    }
}
