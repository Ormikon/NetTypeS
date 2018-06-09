using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetTypeS.Interfaces;

namespace NetTypeS.Types
{
    internal sealed class ComplexTypeProperty : ITypeProperty
    {
        private readonly Lazy<Attribute[]> _customAttributes;

        public ComplexTypeProperty(MemberInfo memberInfo)
        {
            if (memberInfo.MemberType == MemberTypes.Field)
            {
                var fi = (FieldInfo)memberInfo;
                Type = fi.FieldType;
                CanRead = true;
                CanWrite = !(fi.IsInitOnly || fi.IsLiteral);
            }
            else if (memberInfo.MemberType == MemberTypes.Property)
            {
                var pi = (PropertyInfo)memberInfo;
                Type = pi.PropertyType;
                CanRead = pi.CanRead;
                CanWrite = pi.CanWrite;
            }
            else
            {
                throw new ArgumentException("Invalid member type. It should be field or property.", "memberInfo");
            }

            Name = memberInfo.Name;
            _customAttributes = new Lazy<Attribute[]>(() =>
                                                     {
                                                         var ca = memberInfo.GetCustomAttributes();
                                                         return ca == null
                                                             ? new Attribute[0]
                                                             : ca.ToArray();
                                                     });
        }

        public IReadOnlyCollection<Attribute> GetCustomAttributes() => _customAttributes.Value;

        public IEnumerable<Attribute> GetCustomAttributes(Type attributeType) => GetCustomAttributes().Where(at => at.GetType() == attributeType);

        public IEnumerable<T> GetCustomAttributes<T>() where T : Attribute => GetCustomAttributes().OfType<T>();

        public string Name { get; }

        public bool CanWrite { get; }

        public bool CanRead { get; }

        public Type Type { get; }
    }
}