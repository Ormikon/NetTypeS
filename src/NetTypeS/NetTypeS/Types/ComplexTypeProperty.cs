using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetTypeS.Interfaces;

namespace NetTypeS.Types
{
	internal sealed class ComplexTypeProperty : ITypeProperty
	{
		private readonly Type propertyType;
		private readonly bool canRead;
		private readonly bool canWrite;
		private readonly Lazy<Attribute[]> customAttributes;
		private readonly string name;

		public ComplexTypeProperty(MemberInfo memberInfo)
		{
			if (memberInfo.MemberType == MemberTypes.Field)
			{
				var fi = (FieldInfo) memberInfo;
				propertyType = fi.FieldType;
				canRead = true;
				canWrite = !(fi.IsInitOnly || fi.IsLiteral);
			}
			else if (memberInfo.MemberType == MemberTypes.Property)
			{
				var pi = (PropertyInfo) memberInfo;
				propertyType = pi.PropertyType;
				canRead = pi.CanRead;
				canWrite = pi.CanWrite;
			}
			else
			{
				throw new ArgumentException("Invalid member type. It should be field or property.", "memberInfo");
			}

			name = memberInfo.Name;
			customAttributes = new Lazy<Attribute[]>(() =>
			                                         {
				                                         var ca = memberInfo.GetCustomAttributes();
				                                         return ca == null
					                                         ? new Attribute[0]
					                                         : ca.ToArray();
			                                         });
		}

		public IReadOnlyCollection<Attribute> GetCustomAttributes()
		{
			return customAttributes.Value;
		}

		public IEnumerable<Attribute> GetCustomAttributes(Type attributeType)
		{
			return GetCustomAttributes().Where(at => at.GetType() == attributeType);
		}

		public IEnumerable<T> GetCustomAttributes<T>() where T : Attribute
		{
			return GetCustomAttributes().OfType<T>();
		}

		public string Name
		{
			get { return name; }
		}

		public bool CanWrite
		{
			get { return canWrite; }
		}

		public bool CanRead
		{
			get { return canRead; }
		}

		public Type Type
		{
			get { return propertyType; }
		}
	}
}