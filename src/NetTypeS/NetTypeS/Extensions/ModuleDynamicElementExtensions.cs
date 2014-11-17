using System;
using System.Linq;
using NetTypeS.Delegates;
using NetTypeS.Elements.Dynamic;
using NetTypeS.Interfaces;

// ReSharper disable CheckNamespace

namespace NetTypeS
// ReSharper restore CheckNamespace
{
	public static class ModuleDynamicElementExtensions
	{
		/// <summary>
		/// Adds a new element to the module.
		/// </summary>
		/// <param name="generatorModule">Generator module.</param>
		/// <param name="element">TypeScript element.</param>
		/// <returns>Current module builder.</returns>
		public static IGeneratorModule Element(this IGeneratorModule generatorModule, ITypeScriptElement element)
		{
			if (element == null)
				throw new ArgumentNullException("element");
			generatorModule.DynamicElements.Add(new SingleDynamicElement(element));
			return generatorModule;
		}

		/// <summary>
		/// Adds a new element returned from the delegate. Delegate will execute for every interface in the module.
		/// </summary>
		/// <param name="generatorModule">Generator module.</param>
		/// <param name="builder">Element builder.</param>
		/// <returns>Current module builder.</returns>
		public static IGeneratorModule ForInterfaces(this IGeneratorModule generatorModule,
			CustomInterfaceElementBuilder builder)
		{
			if (builder != null)
				generatorModule.DynamicElements.Add(new DynamicElement(
					() => generatorModule.Generator.TypeCollector
						.GetComplexTypes(generatorModule.FullName)
						.Select(ct => builder(ct))));
			return generatorModule;
		}

		/// <summary>
		/// Adds a new element returned from the delegate. Delegate will execute for every interface in the module.
		/// </summary>
		/// <param name="generatorModule">Generator module.</param>
		/// <param name="builder">Element builder.</param>
		/// <returns>Current module builder.</returns>
		public static IGeneratorModule ForInterfaces(this IGeneratorModule generatorModule,
			CustomTypeElementBuilder builder)
		{
			if (builder != null)
				generatorModule.DynamicElements.Add(new DynamicElement(
					() => generatorModule.Generator.TypeCollector
						.GetComplexTypes(generatorModule.FullName)
						.Select(ct => builder(ct.Type))));
			return generatorModule;
		}

		/// <summary>
		/// Adds a new element returned from the delegate. Delegate will execute for the pointed type
		/// if it will be found in interfaces collection.
		/// </summary>
		/// <param name="generatorModule">Generator module.</param>
		/// <param name="interfaceType">Type of interface.</param>
		/// <param name="builder">Element builder.</param>
		/// <returns>Current module builder.</returns>
		public static IGeneratorModule ForInterface(this IGeneratorModule generatorModule, Type interfaceType,
			CustomInterfaceElementBuilder builder)
		{
			if (interfaceType != null && builder != null)
				generatorModule.DynamicElements.Add(new SingleDynamicElement(
					() =>
					{
						var ct = generatorModule.Generator.TypeCollector.Get<IComplexType>(interfaceType);
						return ct == null ? null : builder(ct);
					}));
			return generatorModule;
		}

		/// <summary>
		/// Adds a new element returned from the delegate. Delegate will execute for every enum in the module.
		/// </summary>
		/// <param name="generatorModule">Generator module.</param>
		/// <param name="builder">Element builder.</param>
		/// <returns>Current module builder.</returns>
		public static IGeneratorModule ForEnums(this IGeneratorModule generatorModule, CustomEnumElementBuilder builder)
		{
			if (builder != null)
				generatorModule.DynamicElements.Add(new DynamicElement(
					() => generatorModule.Generator.TypeCollector
						.GetEnumTypes(generatorModule.FullName)
						.Select(et => builder(et))));
			return generatorModule;
		}

		/// <summary>
		/// Adds a new element returned from the delegate. Delegate will execute for every enum in the module.
		/// </summary>
		/// <param name="generatorModule">Generator module.</param>
		/// <param name="builder">Element builder.</param>
		/// <returns>Current module builder.</returns>
		public static IGeneratorModule ForEnums(this IGeneratorModule generatorModule, CustomTypeElementBuilder builder)
		{
			if (builder != null)
				generatorModule.DynamicElements.Add(new DynamicElement(
					() => generatorModule.Generator.TypeCollector
						.GetEnumTypes(generatorModule.FullName)
						.Select(et => builder(et.Type))));
			return generatorModule;
		}

		/// <summary>
		/// Adds a new element returned from the delegate. Delegate will execute for the pointed type
		/// if it will be found in enums collection.
		/// </summary>
		/// <param name="generatorModule">Generator module.</param>
		/// <param name="enumType">Type of Enum.</param>
		/// <param name="builder">Element builder.</param>
		/// <returns>Current module builder.</returns>
		public static IGeneratorModule ForEnum(this IGeneratorModule generatorModule, Type enumType,
			CustomEnumElementBuilder builder)
		{
			if (enumType != null && builder != null)
				generatorModule.DynamicElements.Add(new SingleDynamicElement(
					() =>
					{
						var et = generatorModule.Generator.TypeCollector.Get<IEnumType>(enumType);
						return et == null ? null : builder(et);
					}));
			return generatorModule;
		}

		/// <summary>
		/// Adds a new element returned from the delegate. Delegate will execute for the pointed type.
		/// </summary>
		/// <param name="generatorModule">Generator module.</param>
		/// <param name="type">Type of interface or enum.</param>
		/// <param name="builder">Element builder.</param>
		/// <returns>Current module builder.</returns>
		public static IGeneratorModule For(this IGeneratorModule generatorModule, Type type,
			CustomTypeElementBuilder builder)
		{
			if (type != null && builder != null)
				generatorModule.DynamicElements.Add(new SingleDynamicElement(
					() =>
					{
						var tst = generatorModule.Generator.TypeCollector.Get(type);
						return tst == null ? null : builder(type);
					}));
			return generatorModule;
		}

		/// <summary>
		/// Adds a new element returned from the delegate. Delegate will execute for the pointed type.
		/// </summary>
		/// <param name="generatorModule">Generator module.</param>
		/// <param name="type">Type of interface or enum.</param>
		/// <param name="builder">Element builder.</param>
		/// <returns>Current module builder.</returns>
		public static IGeneratorModule For(this IGeneratorModule generatorModule, Type type,
			CustomTypeScriptElementBuilder builder)
		{
			if (type != null && builder != null)
				generatorModule.DynamicElements.Add(new SingleDynamicElement(
					() =>
					{
						var tst = generatorModule.Generator.TypeCollector.Get(type);
						return tst == null ? null : builder(tst);
					}));
			return generatorModule;
		}

		/// <summary>
		/// Adds a new element returned from the delegate. Delegate will execute for the pointed type
		/// if it will be found in interfaces collection.
		/// </summary>
		/// <typeparam name="T">Type of interface.</typeparam>
		/// <param name="generatorModule">Generator module.</param>
		/// <param name="builder">Element builder.</param>
		/// <returns>Current module builder.</returns>
		public static IGeneratorModule ForInterface<T>(this IGeneratorModule generatorModule,
			CustomInterfaceElementBuilder builder)
		{
			return generatorModule.ForInterface(typeof (T), builder);
		}

		/// <summary>
		/// Adds a new element returned from the delegate. Delegate will execute for the pointed type
		/// if it will be found in enums collection.
		/// </summary>
		/// <typeparam name="T">Type of Enum.</typeparam>
		/// <param name="generatorModule">Generator module.</param>
		/// <param name="builder">Element builder.</param>
		/// <returns>Current module builder.</returns>
		public static IGeneratorModule ForEnum<T>(this IGeneratorModule generatorModule, CustomEnumElementBuilder builder)
		{
			return generatorModule.ForEnum(typeof (T), builder);
		}

		/// <summary>
		/// Adds a new element returned from the delegate. Delegate will execute for the pointed type.
		/// </summary>
		/// <typeparam name="T">Type of interface or enum.</typeparam>
		/// <param name="generatorModule">Generator module.</param>
		/// <param name="builder">Element builder.</param>
		/// <returns>Current module builder.</returns>
		public static IGeneratorModule For<T>(this IGeneratorModule generatorModule, CustomTypeElementBuilder builder)
		{
			return generatorModule.For(typeof (T), builder);
		}

		/// <summary>
		/// Adds a new element returned from the delegate. Delegate will execute for the pointed type.
		/// </summary>
		/// <typeparam name="T">Type of interface or enum.</typeparam>
		/// <param name="generatorModule">Generator module.</param>
		/// <param name="builder">Element builder.</param>
		/// <returns>Current module builder.</returns>
		public static IGeneratorModule For<T>(this IGeneratorModule generatorModule, CustomTypeScriptElementBuilder builder)
		{
			return generatorModule.For(typeof (T), builder);
		}
	}
}