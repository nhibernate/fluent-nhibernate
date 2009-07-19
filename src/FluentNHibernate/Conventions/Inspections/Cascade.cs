namespace FluentNHibernate.Conventions.Inspections
{
    public class Cascade
    {
        public static readonly Cascade Unset = new Cascade("");
        public static readonly Cascade All = new Cascade("all");
        public static readonly Cascade AllDeleteOrphan = new Cascade("all-delete-orphan");
        public static readonly Cascade None = new Cascade("none");
        public static readonly Cascade SaveUpdate = new Cascade("save-update");
        public static readonly Cascade Delete = new Cascade("delete");
        
        private readonly string value;

        private Cascade(string value)
        {
            this.value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Cascade) return Equals((Cascade)obj);

            return base.Equals(obj);
        }

        public bool Equals(Cascade other)
        {
            return Equals(other.value, value);
        }

        public override int GetHashCode()
        {
            return (value != null ? value.GetHashCode() : 0);
        }

        public static bool operator ==(Cascade x, Cascade y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Cascade x, Cascade y)
        {
            return !(x == y);
        }

        public override string ToString()
        {
            return value;
        }

        public static Cascade FromString(string value)
        {
            return new Cascade(value);
        }
    }
}