using System;

namespace FluentNHibernate.Conventions.Inspections
{
    public class Include
    {
        public static readonly Include Unset = new Include("");
        public static readonly Include All = new Include("all");
        public static readonly Include NonLazy = new Include("non-lazy");

        public static Include Custom(string value)
        {
            return new Include(value);
        }

        private readonly string value;

        private Include(string value)
        {
            this.value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Include) return Equals((Include)obj);

            return base.Equals(obj);
        }

        public bool Equals(Include other)
        {
            return Equals(other.value, value);
        }

        public override int GetHashCode()
        {
            return (value != null ? value.GetHashCode() : 0);
        }

        public static bool operator ==(Include x, Include y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Include x, Include y)
        {
            return !(x == y);
        }

        public override string ToString()
        {
            return value;
        }

        public static Include FromString(string value)
        {
            return new Include(value);
        }
    }
}