﻿using System;
using System.Collections.Generic;
using System.Linq;
using NetTypeS.Interfaces;
using NetTypeS.Types;
using NetTypeS.Utils;

namespace NetTypeS
{
    /// <summary>
    /// TypeScript generator
    /// </summary>
    public sealed class Generator : IGenerator
    {
        private readonly IList<string> _modulesOrder = new List<string>();

        private readonly IDictionary<string, IGeneratorModule> modules =
            new Dictionary<string, IGeneratorModule>(StringComparer.Ordinal);

        private Generator(GeneratorSettings settings)
        {
            this.Settings = settings.Clone();
            InheritedTypeSpy = new InheritedTypeSpy(settings.InheritedTypeAssemblies.ToArray());
            TypeCollector = new TypeCollector(InheritedTypeSpy,
                settings.IncludeInheritedTypes,
                settings.GenerateNumberTypeForDictionaryKeys
            );
        }

        /// <summary>
        /// Creates a new instance of TypeScript generator
        /// </summary>
        /// <param name="settings">Generator settings</param>
        /// <returns>TypeScript generator.</returns>
        public static IGenerator New(GeneratorSettings settings = null)
        {
            return new Generator(settings ?? new GeneratorSettings());
        }

        #region IGenerator

        public IGeneratorModule GetModule(string moduleName, bool decalration, bool export)
        {
            if (moduleName == null)
                moduleName = "";
            IGeneratorModule module;
            if (modules.TryGetValue(moduleName, out module))
                return module;
            IGeneratorModule parent = null;
            if (!string.IsNullOrEmpty(moduleName))
            {
                var dotIdx = moduleName.LastIndexOf('.');
                if (dotIdx >= 0 && dotIdx != (moduleName.Length - 1))
                {
                    var parentModuleName = moduleName.Remove(dotIdx);
                    moduleName = moduleName.Substring(dotIdx + 1);
                    parent = GetModule(parentModuleName, decalration, export);
                }
                else
                    parent = GetModule("", decalration, export);
            }

            module = new GeneratorModule(moduleName, parent, this, decalration, export);
            modules.Add(module.FullName, module);
            _modulesOrder.Add(module.FullName);
            return module;
        }

        private static GeneratorModulesTree[] GetChildNodes(string moduleName, IDictionary<string, IList<string>> moduleChild)
        {
            IList<string> child;
            return !moduleChild.TryGetValue(moduleName, out child)
                ? null
                : child.Select(c => new GeneratorModulesTree { Root = c, Child = GetChildNodes(c, moduleChild) })
                    .ToArray();
        }

        private IEnumerable<GeneratorModulesTree> BuildModulesTree(IList<string> moduleNames)
        {
            var s = new HashSet<string>(moduleNames);
            var rootModules = new HashSet<string>();
            var moduleChild = new Dictionary<string, IList<string>>();
            foreach (var moduleName in moduleNames)
            {
                var m = modules[moduleName];
                var parent = m.Parent;
                while (parent != null && !s.Contains(parent.FullName))
                {
                    parent = parent.Parent;
                }
                if (parent == null)
                {
                    rootModules.Add(moduleName);
                    continue;
                }
                IList<string> childList;
                if (!moduleChild.TryGetValue(parent.FullName, out childList))
                {
                    childList = new List<string>();
                    moduleChild.Add(parent.FullName, childList);
                }
                childList.Add(moduleName);
            }
            return
                moduleNames.Where(rootModules.Contains)
                    .Select(mn => new GeneratorModulesTree { Root = mn, Child = GetChildNodes(mn, moduleChild) })
                    .ToArray();
        }

        private void GenerateNamespace(GeneratorModulesTree tree, string parentModuleName, IGeneratorModuleContext context)
        {
            var m = modules[tree.Root ?? ""];
            var el = (ITypeScriptElement)m;
            if (string.IsNullOrEmpty(tree.Root))
            {
                if (tree.Child != null)
                    foreach (var child in tree.Child)
                    {
                        GenerateNamespace(child, tree.Root, context);
                    }
                el.Generate(context);
            }
            else
            {
                var moduleName = string.IsNullOrEmpty(parentModuleName)
                    ? tree.Root
                    : tree.Root.Substring(parentModuleName.Length + 1);
                if (m.Declaration)
                    context.Builder.Append("declare ");
                else if (m.Export)
                    context.Builder.Append("export ");
                context.Builder.Append("module ");
                context.Builder.Append(context.Formatter.FormatModuleName(moduleName));
                context.Builder.AppendLine(" {");
                using (context.Builder.Indent())
                {
                    if (tree.Child != null)
                        foreach (var child in tree.Child)
                        {
                            GenerateNamespace(child, tree.Root, context);
                        }
                    el.Generate(context);
                }
                context.Builder.AppendLine("}");
                context.Builder.AppendLine();
            }
        }

        private void GenerateReferences(ScriptBuilder builder)
        {
            if (References.Count > 0)
                foreach (var reference in References)
                {
                    builder.Append("/// <reference path=\"");
                    builder.Append(reference);
                    builder.AppendLine("\" />");
                }
        }

        public string GenerateNamespaces(params string[] exportedNamespaces)
        {
            var builder = new ScriptBuilder(Settings.Format.Indent, Settings.Format.IndentChar,
                Settings.Format.IndentSize);

            GenerateReferences(builder);

            var context = new GeneratorModuleContext(null, builder, TypeCollector, CustomTypeNameHolder, Settings);
            var modulesForBuild = _modulesOrder;

            if (exportedNamespaces != null && exportedNamespaces.Length > 0)
            {
                var filter = new HashSet<string>(exportedNamespaces.Where(m => m != null).Distinct());
                modulesForBuild = _modulesOrder.Where(filter.Contains).ToArray();
            }

            if (modulesForBuild.Count == 0)
                return builder.ToString();

            var tree = BuildModulesTree(modulesForBuild);

            foreach (var modulesTree in tree)
            {
                GenerateNamespace(modulesTree, null, context);
            }

            return builder.ToString();
        }

        public string GenerateModule(string moduleName)
        {
            var builder = new ScriptBuilder(Settings.Format.Indent, Settings.Format.IndentChar,
                Settings.Format.IndentSize);

            GenerateReferences(builder);

            var context = new GeneratorModuleContext(null, builder, TypeCollector, CustomTypeNameHolder, Settings);

            var module = (ITypeScriptElement)modules[moduleName];
            module.Generate(context);

            return builder.ToString();
        }

        public ICustomTypeNameHolder CustomTypeNameHolder { get; } = new CustomTypeNameHolder();

        public ITypeCollector TypeCollector { get; }

        public IInheritedTypeSpy InheritedTypeSpy { get; }

        public IGeneratorSettings Settings { get; }

        public ICollection<string> References { get; } = new List<string>();

        #endregion
    }
}