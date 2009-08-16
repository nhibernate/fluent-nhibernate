namespace FluentNHibernate.Conventions.Inspections
{
    public class Generated
    {
        public static readonly Generated Unset = new Generated("");
        public static readonly Generated Never = new Generated("never");
        public static readonly Generated Insert = new Generated("insert");
        public static readonly Generated Always = new Generated("always");

        private readonly string value;

        private Generated(string value)
        {
            this.value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Generated) return Equals((Generated)obj);

            return base.Equals(obj);
        }

        public bool Equals(Generated other)
        {
            return Equals(other.value, value);
        }

        public override int GetHashCode()
        {
            return (value != null ? value.GetHashCode() : 0);
        }

        public static bool operator ==(Generated x, Generated y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Generated x, Generated y)
        {
            return !(x == y);
        }

        public override string ToString()
        {
            return value;
        }

        public static Generated FromString(string value)
        {
            return new Generated(value);
        }
    }
}