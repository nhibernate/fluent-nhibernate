namespace FluentNHibernate.Conventions.Inspections
{
    public class OptimisticLock
    {
        public static readonly OptimisticLock Unset = new OptimisticLock("");
        public static readonly OptimisticLock None = new OptimisticLock("none");
        public static readonly OptimisticLock Version = new OptimisticLock("version");
        public static readonly OptimisticLock Dirty = new OptimisticLock("dirty");
        public static readonly OptimisticLock All = new OptimisticLock("all");

        private readonly string value;

        private OptimisticLock(string value)
        {
            this.value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is OptimisticLock) return Equals((OptimisticLock)obj);

            return base.Equals(obj);
        }

        public bool Equals(OptimisticLock other)
        {
            return Equals(other.value, value);
        }

        public override int GetHashCode()
        {
            return (value != null ? value.GetHashCode() : 0);
        }

        public static bool operator ==(OptimisticLock x, OptimisticLock y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(OptimisticLock x, OptimisticLock y)
        {
            return !(x == y);
        }

        public override string ToString()
        {
            return value;
        }

        public static OptimisticLock FromString(string value)
        {
            return new OptimisticLock(value);
        }
    }
}