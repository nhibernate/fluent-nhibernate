namespace FluentNHibernate.Conventions.Inspections
{
    public class Access
    {
        public static readonly Access Unset = new Access("");

        private string value;

        private Access(string value)
        {
            this.value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Access) return Equals((Access)obj);

            return base.Equals(obj);
        }

        public bool Equals(Access other)
        {
            return Equals(other.value, value);
        }

        public override int GetHashCode()
        {
            return (value != null ? value.GetHashCode() : 0);
        }

        public static Access FromString(string value)
        {
            return new Access(value);
        }

        public static Access AsField()
        {
            return FromString("field");
        }
    }
}