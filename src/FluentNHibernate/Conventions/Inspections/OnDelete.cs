namespace FluentNHibernate.Conventions.Inspections
{
    public class OnDelete
    {
        public static readonly OnDelete Unset = new OnDelete("");
        public static readonly OnDelete Cascade = new OnDelete("cascade");
        public static readonly OnDelete NoAction = new OnDelete("noaction");

        private readonly string value;

        private OnDelete(string value)
        {
            this.value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is OnDelete) return Equals((OnDelete)obj);

            return base.Equals(obj);
        }

        public bool Equals(OnDelete other)
        {
            return Equals(other.value, value);
        }

        public override int GetHashCode()
        {
            return (value != null ? value.GetHashCode() : 0);
        }

        public static bool operator ==(OnDelete x, OnDelete y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(OnDelete x, OnDelete y)
        {
            return !(x == y);
        }

        public override string ToString()
        {
            return value;
        }

        public static OnDelete FromString(string value)
        {
            return new OnDelete(value);
        }
    }
}