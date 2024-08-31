namespace Alpha.Framework.MediatR.EventSourcing.Entity
{
    public abstract class TypedIdValueBase : IEquatable<TypedIdValueBase>, IComparable
    {
        public Guid Value { get; }

        public TypedIdValueBase(string value)
        {
            if (value == null) return;
            Value = Guid.Parse(value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TypedIdValueBase other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public bool Equals(TypedIdValueBase other)
        {
            if (other == null) return false;
            return this.Value == other.Value;
        }

        public static bool operator ==(TypedIdValueBase obj1, TypedIdValueBase obj2)
        {
            if (object.Equals(obj1, null))
            {
                if (object.Equals(obj2, null))
                {
                    return true;
                }
                return false;
            }
            return obj1.Equals(obj2);
        }

        public static bool operator !=(TypedIdValueBase x, TypedIdValueBase y)
        {
            return !(x == y);
        }

        public override string ToString()
        {
            if (this.Value == null) return null;
            return this.Value.ToString();
        }

        public int CompareTo(object obj)
        {
            if (this.Value == null || obj == null) return 0;

            if (obj is TypedIdValueBase)
            {
                var objToCompare = (TypedIdValueBase)obj;
                if (objToCompare.Value == null) return 0;

                return String.Compare(this.Value.ToString(), objToCompare.Value.ToString());
            }
            return 0;
        }
    }
}
