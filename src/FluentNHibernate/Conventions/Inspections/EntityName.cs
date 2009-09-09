namespace FluentNHibernate.Conventions.Inspections
{
    public class EntityName
    {
        public static readonly EntityName Unset = new EntityName("");

        private readonly string value;

		private EntityName(string value)
        {
            this.value = value;
        }

        public override bool Equals(object obj)
        {
			if (obj is EntityName) return Equals((EntityName)obj);

            return base.Equals(obj);
        }

		public bool Equals(EntityName other)
        {
            return Equals(other.value, value);
        }

        public override int GetHashCode()
        {
            return (value != null ? value.GetHashCode() : 0);
        }

		public static bool operator ==(EntityName x, EntityName y)
        {
            return x.Equals(y);
        }

		public static bool operator !=(EntityName x, EntityName y)
        {
            return !(x == y);
        }

        public override string ToString()
        {
            return value;
        }

		public static EntityName FromString(string value)
        {
			return new EntityName(value);
        }
    }
}