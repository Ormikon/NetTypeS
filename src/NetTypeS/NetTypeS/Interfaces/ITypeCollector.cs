using System;
using System.Collections.Generic;

namespace NetTypeS.Interfaces
{
	public interface ITypeCollector
	{
		void Collect(Type type, string moduleBinding, bool overrideBindingIfExists = true);
		void Replace(Type type, Type withType);
		void Replace(Func<Type, bool> test, Type withType);
		ITypeScriptType Get(Type type);
		string GetModuleBinding(Type type);
		IEnumerable<IComplexType> GetComplexTypes();
		IEnumerable<IComplexType> GetComplexTypes(string module);
		IEnumerable<IEnumType> GetEnumTypes();
		IEnumerable<IEnumType> GetEnumTypes(string module);
		IEnumerable<ITypeScriptType> GetTypes(string module);
		IEnumerable<ITypeScriptType> Collected { get; }
		IEnumerable<Type> CollectedTypes { get; }
	}
}