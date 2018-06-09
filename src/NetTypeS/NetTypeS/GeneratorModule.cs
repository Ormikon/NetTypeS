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
        public GeneratorModule(string name, IGeneratorModule parent, IGenerator generator, bool declaration, bool export)
        {
            Name = name;
            Parent = parent;
            Generator = generator;
            Declaration = declaration;
            Export = export;
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
            return Parent == null;
        }

        public IGenerator Generator { get; }

        public IGeneratorModule Parent { get; }

        public ICollection<IDynamicElement> DynamicElements { get; } = new List<IDynamicElement>();

        public ICollection<ModuleImport> Imports { get; } = new List<ModuleImport>();

        public string Name { get; }

        public string FullName => GetModuleFullName(Parent, Name);

        public bool Declaration { get; }

        public bool Export { get; }

        #endregion

        #region ITypeScriptElement

        /// <summary>
        /// Is Enums and Interfaces should be exported.
        /// </summary>
        /// <returns>Exported or not</returns>
        private bool IsTypesExported()
        {
            // Will be exported if not declaration or root module
            return !Declaration && !IsRootModule();
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
            var moduleContext = new GeneratorModuleContext(FullName, context.Builder, Generator.TypeCollector,
                Generator.CustomTypeNameHolder, Generator.Settings);

            if (Imports.Count > 0)
            {
                foreach (var import in Imports)
                {
                    if (import.Alias != null)
                    {
                        context.Builder.AppendLine($"import * as {import.Alias} from '{import.Module}';");
                    }
                    else
                    {
                        context.Builder.AppendLine($"import {{ { string.Join(", ", import.Bindings) } }} from '{import.Module}';");
                    }
                }
                context.Builder.AppendLine();
            }

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
            var complexTypes = Generator.TypeCollector.GetComplexTypes(FullName).Cast<ComplexType>().ToArray();
            var enums = Generator.TypeCollector.GetEnumTypes(FullName).Cast<EnumType>().ToArray();
            if (complexTypes.Length == 0 && enums.Length == 0 && DynamicElements.Count == 0)
                return EmptyElement.EmptyEnumerable;

            return BuildEnums(enums)
                .Concat(BuildInterfaces(complexTypes))
                .Concat(DynamicElements.SelectMany(de => de.GetElements().Where(e => e != null)));
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

        public int Count => GetModuleElements().Count();

        public bool IsReadOnly => true;

        #endregion
    }
}