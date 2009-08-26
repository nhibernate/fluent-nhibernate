using System;

namespace FluentNHibernate.Conventions.Inspections
{
    public class SchemaAction
    {
        public static readonly SchemaAction Unset = new SchemaAction("");
        public static readonly SchemaAction Drop = new SchemaAction("drop");
        public static readonly SchemaAction Export = new SchemaAction("export");
        public static readonly SchemaAction None = new SchemaAction("none");
        public static readonly SchemaAction Update = new SchemaAction("update");
        public static readonly SchemaAction Validate = new SchemaAction("validate");
        public static readonly SchemaAction All = new SchemaAction("all");
        
        private readonly string value;

        private SchemaAction(string value)
        {
            this.value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is SchemaAction) return Equals((SchemaAction)obj);

            return base.Equals(obj);
        }

        public bool Equals(SchemaAction other)
        {
            return Equals(other.value, value);
        }

        public override int GetHashCode()
        {
            return (value != null ? value.GetHashCode() : 0);
        }

        public static bool operator ==(SchemaAction x, SchemaAction y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(SchemaAction x, SchemaAction y)
        {
            return !(x == y);
        }

        public override string ToString()
        {
            return value;
        }

        public static SchemaAction FromString(string value)
        {
            return new SchemaAction(value);
        }

        public static SchemaAction Custom(string customValue)
        {
            return new SchemaAction(customValue);
        }
    }
}