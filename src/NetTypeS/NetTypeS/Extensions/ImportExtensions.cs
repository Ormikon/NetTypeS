using System;
using System.Reflection;
using NetTypeS.Interfaces;

// ReSharper disable CheckNamespace

namespace NetTypeS
// ReSharper restore CheckNamespace
{
	public static class ImportExtensions
	{
		/// <summary>
		/// Includes all public types from the assembly with given namespace and attribute into the module.
		/// </summary>
		/// <param name="generatorModule">Generator module.</param>
		/// <param name="assembly">Assembly</param>
		/// <param name="attributeType">Attribute type.</param>
		/// <param name="namespace">Types namespace.</param>
		/// <param name="flattern">Do not include type namespace.</param>
		/// <returns>Current generator module.</returns>
		public static IGeneratorModule IncludeFrom(this IGeneratorModule generatorModule, Assembly assembly,
			Type attributeType = null, string @namespace = null, bool flattern = true)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");
			foreach (var type in assembly.GetTypes())
			{
				if (!type.IsPublic)
					continue;
				// if static
				if (type.IsClass && type.IsSealed && type.IsAbstract)
					continue;
				if (typeof (Delegate).IsAssignableFrom(type))
					continue;
				if (attributeType != null && !type.IsDefined(attributeType))
					continue;
				if (!string.IsNullOrEmpty(@namespace) && !(type.Namespace ?? "").StartsWith(@namespace))
					continue;
				if (flattern)
					generatorModule.Include(type);
				else
				{
					var t = type;
					var ns = type.Namespace;
					bool exportModule = false;
					if (!string.IsNullOrEmpty(ns))
					{
						var nsSeparator = ns.IndexOf('.');
						if (nsSeparator > 0)
						{
							generatorModule.Generator.GetModule(ns.Remove(nsSeparator));
							exportModule = true;
						}
					}
					var module = generatorModule.Generator.GetModule(t.Namespace, export: exportModule);
					module.Include(t);
				}
			}

			return generatorModule;
		}

		/// <summary>
		/// Includes all public types from the calling assembly with given namespace and attribute into the module.
		/// </summary>
		/// <param name="generatorModule">Generator module.</param>
		/// <param name="attributeType">Attribute type.</param>
		/// <param name="namespace">Types namespace.</param>
		/// <param name="flattern">Do not include type namespace.</param>
		/// <returns>Current generator module.</returns>
		public static IGeneratorModule IncludeFromCurrentAssembly(this IGeneratorModule generatorModule,
			Type attributeType = null, string @namespace = null, bool flattern = true)
		{
			Assembly current = null;
			try
			{
				current = Assembly.GetCallingAssembly();
			}
// ReSharper disable EmptyGeneralCatchClause
			catch (Exception)
			{
			}
// ReSharper restore EmptyGeneralCatchClause
			if (current == null)
				return generatorModule;
			return generatorModule.IncludeFrom(current, attributeType, @namespace, flattern);
		}
	}
}