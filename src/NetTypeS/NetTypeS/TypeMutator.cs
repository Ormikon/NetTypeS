using System;

namespace NetTypeS
{
    internal class TypeMutator : IEquatable<TypeMutator>
    {
        private readonly Func<Type, bool> _canMutate;
        private readonly Type _replacement;

        public TypeMutator(Func<Type, bool> canMutate, Type replacement)
        {
            _canMutate = canMutate ?? throw new ArgumentNullException("canMutate");
            _replacement = replacement ?? throw new ArgumentNullException("replacement");
        }

        public bool TryMutate(Type type, out Type mutated)
        {
            // prevent success result if no mutation
            mutated = type;
            if (type == _replacement)
                return false;
            if (_canMutate(type))
            {
                mutated = _replacement;
                return true;
            }
            return false;
        }

        public bool Equals(TypeMutator other)
        {
            return _canMutate == other._canMutate && _replacement == other._replacement;
        }

        public override int GetHashCode()
        {
            return _canMutate.GetHashCode() ^ _replacement.GetHashCode();
        }
    }
}