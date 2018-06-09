using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NetTypeS.Interfaces;
using NetTypeS.Utils;

namespace NetTypeS.Types
{
    internal sealed class ComplexType : IComplexType
    {
        private readonly Lazy<Attribute[]> customAttributes;

        public ComplexType(Type type)
        {
            IsRequired = false;
            if (type.IsValueType)
            {
                IsValueType = true;
                IsRequired = true;
                if (type.IsNullable())
                {
                    throw new NotSupportedException("Nullable types are not supported in this class.");
                }
            }
            else
                IsValueType = false;

            this.Type = type;
            Name = type.IsGenericType ? GetGenericTypeName(type.Name) : type.Name;
            Namespace = type.Namespace;

            IsGenerated = type.IsDefined(typeof(CompilerGeneratedAttribute), false);

            Interfaces = type.GetInterfaces();
            IsInterface = type.IsInterface;
            Properties = type.GetPublicMembers().Select(mi => new ComplexTypeProperty(mi)).ToArray();
            customAttributes = new Lazy<Attribute[]>(() => type.GetCustomAttributes<Attribute>().ToArray());
        }

        private static string GetGenericTypeName(string name)
        {
            int generatedIdx = name.IndexOf('`');
            return generatedIdx < 0 ? name : name.Remove(generatedIdx);
        }

        public override string ToString()
        {
            return "Complex[" + (IsValueType ? "struct" : "class") + "](" + Name + "[" +
                   string.Join("; ",
                       Properties.Select(
                           p =>
                               p.Name + "{" + (p.CanRead ? "get" + (p.CanWrite ? ";set" : "") : p.CanWrite ? "set" : "") +
                               "}")) +
                   "])";
        }

        public Type Type { get; }

        public string Name { get; }

        public string Namespace { get; }

        public string FullName => Namespace + "." + Name;

        public bool IsValueType { get; }

        public bool IsGenerated { get; }

        public bool IsGeneric => Type.IsGenericType;

        public bool IsGenericDefinition => Type.IsGenericTypeDefinition;

        public bool IsInterface { get; }

        public Type GenericType => Type.GetGenericTypeDefinition();

        public Type[] GenericArguments => Type.GetGenericArguments();

        public Type[] Interfaces { get; }

        public IReadOnlyList<ITypeProperty> Properties { get; }

        public TypeScriptTypeCode Code => TypeScriptTypeCode.Complex;

        public bool IsRequired { get; }

        public Attribute[] CustomAttributes => customAttributes.Value;
    }
}