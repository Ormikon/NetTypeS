using System;
using NetTypeS.Delegates;
using NetTypeS.Interfaces;
using NetTypeS.Types;


// ReSharper disable CheckNamespace

namespace NetTypeS
// ReSharper restore CheckNamespace
{
	public static class GenericExtensions
	{
		/// <summary>
		/// Adds reference on the file.
		/// </summary>
		/// <param name="generator"></param>
		/// <param name="fileName">File name.</param>
		/// <returns>Current generator.</returns>
		public static IGenerator Reference(this IGenerator generator, string fileName)
		{
			generator.References.Add(fileName);
			return generator;
		}

		/// <summary>
		/// Cerates a root module for the builder.
		/// </summary>
		/// <param name="generator">Generator.</param>
		/// <param name="moduleBuilder">Module builder.</param>
		/// <returns>Current generator.</returns>
		public static IGenerator Module(this IGenerator generator, ModuleBuilder moduleBuilder,
			bool export = false)
		{
			return generator.Module(null, moduleBuilder, false, export);
		}

		/// <summary>
		/// Creates a new module it it is not exist.
		/// </summary>
		/// <param name="generator"></param>
		/// <param name="moduleName">Module name.</param>
		/// <param name="moduleBuilder">Module builder.</param>
		/// <param name="decalration">If module should be declared.</param>
		/// <param name="export">If module should be exported.</param>
		/// <returns>Current generator.</returns>
		public static IGenerator Module(this IGenerator generator, string moduleName,
			ModuleBuilder moduleBuilder,
			bool decalration = false, bool export = false)
		{
			var module = generator.GetModule(moduleName, decalration, export);
			if (moduleBuilder != null)
				moduleBuilder(module);
			return generator;
		}

		/// <summary>
		/// Creates a new submodule in the current module or return existing if it exists.
		/// </summary>
		/// <param name="generatorModule">Generator module.</param>
		/// <param name="moduleName">Module name.</param>
		/// <param name="moduleBuilder">Module builder.</param>
		/// <param name="decalration">If module should be declared.</param>
		/// <param name="export">If module should be exported.</param>
		/// <returns>Current module builder.</returns>
		public static IGeneratorModule SubModule(this IGeneratorModule generatorModule, string moduleName,
			ModuleBuilder moduleBuilder, bool decalration = false, bool export = false)
		{
			if (string.IsNullOrWhiteSpace(moduleName))
				throw new ArgumentException("Sub module name cannot be null or empty.", "moduleName");
			var newName = string.IsNullOrEmpty(generatorModule.FullName)
				? moduleName
				: generatorModule.FullName + "." + moduleName;
			generatorModule.Generator.Module(newName, moduleBuilder, decalration, export);
			return generatorModule;
		}

		/// <summary>
		/// Creates a new module with export directive if module is not exist.
		/// </summary>
		/// <param name="generator">Generator.</param>
		/// <param name="moduleName">The name of a new module.</param>
		/// <param name="moduleBuilder">Module builder.</param>
		/// <returns>Current generator.</returns>
		public static IGenerator Export(this IGenerator generator, string moduleName,
			ModuleBuilder moduleBuilder)
		{
			return generator.Module(moduleName, moduleBuilder, false, true);
		}

		/// <summary>
		/// Creates a new module with declare directive if module is not exist.
		/// </summary>
		/// <param name="generator">Generator.</param>
		/// <param name="moduleName">The name of a new module.</param>
		/// <param name="moduleBuilder">Module builder.</param>
		/// <returns>Current generator.</returns>
		public static IGenerator Declare(this IGenerator generator, string moduleName,
			ModuleBuilder moduleBuilder)
		{
			return generator.Module(moduleName, moduleBuilder, true);
		}

		/// <summary>
		/// Creates a new sub module with export directive if module is not exist.
		/// </summary>
		/// <param name="generatorModule">Generator module.</param>
		/// <param name="moduleName">The name of a new module.</param>
		/// <param name="moduleBuilder">Module builder.</param>
		/// <returns>Current generator module.</returns>
		public static IGeneratorModule Export(this IGeneratorModule generatorModule, string moduleName,
			ModuleBuilder moduleBuilder)
		{
			return generatorModule.SubModule(moduleName, moduleBuilder, false, true);
		}

		/// <summary>
		/// Creates a new sub module with declare directive if module is not exist.
		/// </summary>
		/// <param name="generatorModule">Generator module.</param>
		/// <param name="moduleName">The name of a new module.</param>
		/// <param name="moduleBuilder">Module builder.</param>
		/// <returns>Current generator module.</returns>
		public static IGeneratorModule Declare(this IGeneratorModule generatorModule, string moduleName,
			ModuleBuilder moduleBuilder)
		{
			return generatorModule.SubModule(moduleName, moduleBuilder, true);
		}

		/// <summary>
		/// Assign a new name for a type.
		/// </summary>
		/// <param name="generator">Generator.</param>
		/// <param name="type">Type</param>
		/// <param name="name">New name.</param>
		/// <returns>Current generator.</returns>
		public static IGenerator NameFor(this IGenerator generator, Type type, string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Type name cannot be null or empty.");
			generator.CustomTypeNameHolder.RegisterNameFor(type, name);
			return generator;
		}

		/// <summary>
		/// Assign a new name for a type.
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="generator">Generator.</param>
		/// <param name="name">New name.</param>
		/// <returns>Current generator.</returns>
		public static IGenerator NameFor<T>(this IGenerator generator, string name)
		{
			return generator.NameFor(typeof (T), name);
		}

		/// <summary>
		/// Replaces the type with another type.
		/// </summary>
		/// <param name="generator">Generator.</param>
		/// <param name="type">Source type</param>
		/// <param name="withType">Replacement</param>
		/// <returns>Current generator.</returns>
		public static IGenerator Replace(this IGenerator generator, Type type, Type withType)
		{
			generator.TypeCollector.Replace(type, withType);
			return generator;
		}

		/// <summary>
		/// Replaces a type matched with test expression with another type.
		/// </summary>
		/// <param name="generator">Generator.</param>
		/// <param name="test">Type test expression.</param>
		/// <param name="withType">Replacement</param>
		/// <returns>Current generator.</returns>
		public static IGenerator Replace(this IGenerator generator, Func<Type, bool> test, Type withType)
		{
			generator.TypeCollector.Replace(test, withType);
			return generator;
		}

		/// <summary>
		/// Replaces the type with another type.
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="generator">Generator.</param>
		/// <param name="withType">Replacement.</param>
		/// <returns>Current generator.</returns>
		public static IGenerator Replace<T>(this IGenerator generator, Type withType)
		{
			return generator.Replace(typeof (T), withType);
		}

		/// <summary>
		/// Replaces the type with another type.
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <typeparam name="TWith">Replacement</typeparam>
		/// <param name="generator">Generator.</param>
		/// <returns>Current generator.</returns>
		public static IGenerator Replace<T, TWith>(this IGenerator generator)
		{
			return generator.Replace(typeof (T), typeof (TWith));
		}

		/// <summary>
		/// Includes a new type into the module.
		/// </summary>
		/// <param name="generatorModule">Generator module.</param>
		/// <param name="type">Type</param>
		/// <param name="typeName">New name.</param>
		/// <returns>Current module builder.</returns>
		public static IGeneratorModule Include(this IGeneratorModule generatorModule, Type type, string typeName = null)
		{
			generatorModule.Generator.TypeCollector.Collect(type, generatorModule.FullName);
			if (!string.IsNullOrEmpty(typeName))
				generatorModule.Generator.NameFor(type, typeName);
			return generatorModule;
		}

		/// <summary>
		/// Includes a new type into the module.
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="generatorModule">Generator module.</param>
		/// <param name="name">New name.</param>
		/// <returns>Current module builder.</returns>
		public static IGeneratorModule Include<T>(this IGeneratorModule generatorModule, string name = null)
		{
			return generatorModule.Include(typeof (T), name);
		}

		/// <summary>
		/// Includes a new type into the module.
		/// </summary>
		/// <param name="generatorModule">Generator module.</param>
		/// <param name="type">Type</param>
		/// <param name="typeName">New name.</param>
		/// <returns>Current module builder.</returns>
		public static IGeneratorModule IncludeWithInherited(this IGeneratorModule generatorModule, Type type, string typeName = null)
		{
			generatorModule.Generator.TypeCollector.Collect(type, true, generatorModule.FullName);
			if (!string.IsNullOrEmpty(typeName))
				generatorModule.Generator.NameFor(type, typeName);
			return generatorModule;
		}

		/// <summary>
		/// Includes a new type into the module.
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="generatorModule">Generator module.</param>
		/// <param name="name">New name.</param>
		/// <returns>Current module builder.</returns>
		public static IGeneratorModule IncludeWithInherited<T>(this IGeneratorModule generatorModule, string name = null)
		{
			return generatorModule.Include(typeof(T), name);
		}

        /// <summary>
        /// Generates ES6 import clause, using "import { a, b, c } from 'module_path'" syntax.
        /// </summary>
        /// <param name="generatorModule">Generator module</param>
        /// <param name="moduleName">Bindings to generate</param>
        /// <param name="moduleBuilder">Module name (path) to import</param>
        /// <returns>Current generator module.</returns>
        public static IGeneratorModule Import(this IGeneratorModule generatorModule, string[] bindings, string moduleName)
        {
            generatorModule.Imports.Add(new ModuleImport
            {
                Module = moduleName,
                Bindings = bindings
            });

            return generatorModule;
        }

        /// <summary>
        /// Generates ES6 import clause, using "import * as alias from 'module_path'" syntax
        /// </summary>
        /// <param name="generatorModule">Generator module</param>
        /// <param name="alias">Alias name to generate</param>
        /// <param name="moduleBuilder">Module name (path) to import</param>
        /// <returns>Current generator module.</returns>
        public static IGeneratorModule Import(this IGeneratorModule generatorModule, string alias, string moduleName)
        {
            generatorModule.Imports.Add(new ModuleImport
            {
                Module = moduleName,
                Alias = alias
            });

            return generatorModule;
        }

    }
}