namespace FluentNHibernate.Conventions.Inspections
{
    public class Fetch
    {
        public static readonly Fetch Unset = new Fetch("");
        public static readonly Fetch Select = new Fetch("select");
        public static readonly Fetch Join = new Fetch("join");
        
        private readonly string value;

        private Fetch(string value)
        {
            this.value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Fetch) return Equals((Fetch)obj);

            return base.Equals(obj);
        }

        public bool Equals(Fetch other)
        {
            return Equals(other.value, value);
        }

        public override int GetHashCode()
        {
            return (value != null ? value.GetHashCode() : 0);
        }

        public static bool operator ==(Fetch x, Fetch y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Fetch x, Fetch y)
        {
            return !(x == y);
        }

        public override string ToString()
        {
            return value;
        }

        public static Fetch FromString(string value)
        {
            return new Fetch(value);
        }
    }
}