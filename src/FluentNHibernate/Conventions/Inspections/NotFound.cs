namespace FluentNHibernate.Conventions.Inspections
{
    public class NotFound
    {
        public static readonly NotFound Unset = new NotFound("");
        public static readonly NotFound Ignore = new NotFound("ignore");
        public static readonly NotFound Exception = new NotFound("exception");

        private readonly string value;

        private NotFound(string value)
        {
            this.value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is NotFound) return Equals((NotFound)obj);

            return base.Equals(obj);
        }

        public bool Equals(NotFound other)
        {
            return Equals(other.value, value);
        }

        public override int GetHashCode()
        {
            return (value != null ? value.GetHashCode() : 0);
        }

        public static bool operator ==(NotFound x, NotFound y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(NotFound x, NotFound y)
        {
            return !(x == y);
        }

        public override string ToString()
        {
            return value;
        }

        public static NotFound FromString(string value)
        {
            return new NotFound(value);
        }
    }
}