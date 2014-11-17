using System;

namespace NetTypeS
{
	internal class TypeMutator : IEquatable<TypeMutator>
	{
		private readonly Func<Type, bool> canMutate;
		private readonly Type replacement;

		public TypeMutator(Func<Type, bool> canMutate, Type replacement)
		{
			if (canMutate == null)
				throw new ArgumentNullException("canMutate");
			if (replacement == null)
				throw new ArgumentNullException("replacement");

			this.canMutate = canMutate;
			this.replacement = replacement;
		}

		public bool TryMutate(Type type, out Type mutated)
		{
			// prevent success result if no mutation
			mutated = type;
			if (type == replacement)
				return false;
			if (canMutate(type))
			{
				mutated = replacement;
				return true;
			}
			return false;
		}

		public bool Equals(TypeMutator other)
		{
			return canMutate == other.canMutate && replacement == other.replacement;
		}

		public override int GetHashCode()
		{
			return canMutate.GetHashCode() ^ replacement.GetHashCode();
		}
	}
}