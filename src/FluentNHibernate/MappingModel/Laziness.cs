namespace FluentNHibernate.MappingModel
{
    public class Laziness
    {
        private readonly string value;
        public static readonly Laziness True = new Laziness("true");
        public static readonly Laziness False = new Laziness("false");
        public static readonly Laziness Proxy = new Laziness("proxy");

        private Laziness(string value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Laziness) return Equals((Laziness)obj);
            if (obj is bool) return Equals((bool)obj);
            if (obj is string) return Equals((string)obj);

            return base.Equals(obj);
        }

        public bool Equals(Laziness other)
        {
            return Equals(other.value, value);
        }

        public bool Equals(bool other)
        {
            return Equals(other.ToString().ToLowerInvariant(), value);
        }

        public bool Equals(string other)
        {
            return Equals(other, value);
        }

        public override int GetHashCode()
        {
            return (value != null ? value.GetHashCode() : 0);
        }

        public static bool operator ==(Laziness x, Laziness y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Laziness x, Laziness y)
        {
            return !(x == y);
        }

        public static bool operator ==(Laziness x, bool y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Laziness x, bool y)
        {
            return !(x == y);
        }

        public static bool operator ==(Laziness x, string y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Laziness x, string y)
        {
            return !(x == y);
        }

        public static bool operator ==(bool x, Laziness y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(bool x, Laziness y)
        {
            return !(x == y);
        }

        public static bool operator ==(string x, Laziness y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(string x, Laziness y)
        {
            return !(x == y);
        }
    }
}