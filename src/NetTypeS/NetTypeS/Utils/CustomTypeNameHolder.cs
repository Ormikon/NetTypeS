using System;
using System.Collections.Generic;
using NetTypeS.Interfaces;

namespace NetTypeS.Utils
{
	internal class CustomTypeNameHolder : ICustomTypeNameHolder
	{
		private readonly IDictionary<Type, string> registeredNameOverrides =
			new Dictionary<Type, string>();

		public void RegisterNameFor(Type type, string name)
		{
			if (type == null)
				throw new ArgumentNullException("type");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentException("Type name cannot be empty.", "name");

			registeredNameOverrides[type] = name;
		}

		public string GetNameFor(Type type)
		{
			if (type == null)
				return null;
			string name;
			return registeredNameOverrides.TryGetValue(type, out name) ? name : null;
		}
	}
}