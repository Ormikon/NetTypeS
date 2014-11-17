using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NetTypeS.Elements;
using NetTypeS.Elements.Primitives;
using NetTypeS.Interfaces;
using NetTypeS.Types;

namespace NetTypeS
{
	internal class GeneratorModule : ITypeScriptElement, IGeneratorModule
	{
		private readonly ICollection<IDynamicElement> elements = new List<IDynamicElement>();
		private readonly string name;
		private readonly IGeneratorModule parent;
		private readonly IGenerator generator;
		private readonly bool declaration;
		private readonly bool export;

		public GeneratorModule(string name, IGeneratorModule parent, IGenerator generator, bool declaration, bool export)
		{
			this.name = name;
			this.parent = parent;
			this.generator = generator;
			this.declaration = declaration;
			this.export = export;
		}

		#region IGeneratorModule

		private static string GetModuleFullName(string parentFullName, string name)
		{
			return string.IsNullOrEmpty(parentFullName) ? name : parentFullName + "." + name;
		}

		private static string GetModuleFullName(IGeneratorModule parent, string name)
		{
			return parent == null
				? name
				: GetModuleFullName(parent.FullName, name);
		}

		private bool IsRootModule()
		{
			return parent == null;
		}

		public IGenerator Generator
		{
			get { return generator; }
		}

		public IGeneratorModule Parent
		{
			get { return parent; }
		}

		public ICollection<IDynamicElement> DynamicElements
		{
			get { return elements; }
		}

		public string Name
		{
			get { return name; }
		}

		public string FullName
		{
			get { return GetModuleFullName(parent, name); }
		}

		public bool Declaration
		{
			get { return declaration; }
		}

		public bool Export
		{
			get { return export; }
		}

		#endregion

		#region ITypeScriptElement

		/// <summary>
		/// Is Enums and Interfaces should be exported.
		/// </summary>
		/// <returns>Exported or not</returns>
		private bool IsTypesExported()
		{
			// Will be exported if not declaration or root module
			return !declaration && !IsRootModule();
		}

		private IEnumerable<ITypeScriptElement> BuildInterfaces(IEnumerable<ComplexType> complexTypes)
		{
			return complexTypes.Select(ct => new TypeScriptInterface(ct, false, IsTypesExported()));
		}

		private IEnumerable<ITypeScriptElement> BuildEnums(IEnumerable<EnumType> enumTypes)
		{
			return enumTypes.Select(et => new TypeScriptEnum(et, false, IsTypesExported()));
		}

		public void Generate(IGeneratorModuleContext context)
		{
			var moduleContext = new GeneratorModuleContext(FullName, context.Builder, generator.TypeCollector,
				generator.CustomTypeNameHolder, generator.Settings);
			foreach (var element in this)
			{
				element.Generate(moduleContext);
				context.Builder.AppendLine();
				context.Builder.AppendLine();
			}
		}

		#endregion

		#region ICollection of ITypeScriptElement

		private IEnumerable<ITypeScriptElement> GetModuleElements()
		{
			var complexTypes = generator.TypeCollector.GetComplexTypes(FullName).Cast<ComplexType>().ToArray();
			var enums = generator.TypeCollector.GetEnumTypes(FullName).Cast<EnumType>().ToArray();
			if (complexTypes.Length == 0 && enums.Length == 0 && elements.Count == 0)
				return EmptyElement.EmptyEnumerable;

			return BuildEnums(enums)
				.Concat(BuildInterfaces(complexTypes))
				.Concat(elements.SelectMany(de => de.GetElements().Where(e => e != null)));
		}

		public IEnumerator<ITypeScriptElement> GetEnumerator()
		{
			return GetModuleElements().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(ITypeScriptElement item)
		{
			throw new NotSupportedException("This method is not supported the the module element.");
		}

		public void Clear()
		{
			throw new NotSupportedException("This method is not supported the the module element.");
		}

		public bool Contains(ITypeScriptElement item)
		{
			throw new NotSupportedException("This method is not supported the the module element.");
		}

		public void CopyTo(ITypeScriptElement[] array, int arrayIndex)
		{
			GetModuleElements().ToArray().CopyTo(array, arrayIndex);
		}

		public bool Remove(ITypeScriptElement item)
		{
			throw new NotSupportedException("This method is not supported the the module element.");
		}

		public int Count
		{
			get { return GetModuleElements().Count(); }
		}

		public bool IsReadOnly
		{
			get { return true; }
		}

		#endregion
	}
}