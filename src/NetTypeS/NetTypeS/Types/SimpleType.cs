using System;
using System.Threading.Tasks;
using NetTypeS.Interfaces;
using NetTypeS.Utils;

namespace NetTypeS.Types
{
    internal sealed class SimpleType : ITypeScriptType
    {
        public static readonly SimpleType Boolean = new SimpleType(TypeScriptTypeNames.Boolean, true);
        public static readonly SimpleType String = new SimpleType(TypeScriptTypeNames.String);
        public static readonly SimpleType Number = new SimpleType(TypeScriptTypeNames.Number, true);
        public static readonly SimpleType Any = new SimpleType(TypeScriptTypeNames.Any);
        public static readonly SimpleType Void = new SimpleType(TypeScriptTypeNames.Void);

        private SimpleType(string name, bool isRequired = false)
        {
            FullName = name;
            IsRequired = isRequired;
        }

        public static SimpleType Create(Type type)
        {
            if (type.IsGenericParameter)
            {
                return new SimpleType(type.Name, type.IsValueType);
            }
            if (typeof(object) == type)
            {
                return Any;
            }
            if (typeof(void) == type)
            {
                return Void;
            }
            if (type.IsNullable())
            {
                throw new NotSupportedException("Nullable types are not supported in this class.");
            }
            var tc = Type.GetTypeCode(type);
            switch (tc)
            {
                case TypeCode.Boolean:
                    return Boolean;
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return Number;
                case TypeCode.Char:
                case TypeCode.String:
                    return String;
                case TypeCode.Empty:
                    return Void;
                default:
                    throw new ArgumentException("Unsupported simple type " + type.Name + ".", "type");
            }
        }

        public override string ToString()
        {
            return "Simple(" + FullName + ")";
        }

        public string Name => FullName;

        public string Namespace => "";

        public string FullName { get; }

        public TypeScriptTypeCode Code => TypeScriptTypeCode.Simple;

        public bool IsRequired { get; }

        public Attribute[] CustomAttributes => new Attribute[0];
    }
}