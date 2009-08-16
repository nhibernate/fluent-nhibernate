namespace FluentNHibernate.Conventions.Inspections
{
    public class Polymorphism
    {
        public static readonly Polymorphism Unset = new Polymorphism("");
        public static readonly Polymorphism Implicit = new Polymorphism("implicit");
        public static readonly Polymorphism Explicit = new Polymorphism("explicit");

        private readonly string value;

        private Polymorphism(string value)
        {
            this.value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Polymorphism) return Equals((Polymorphism)obj);

            return base.Equals(obj);
        }

        public bool Equals(Polymorphism other)
        {
            return Equals(other.value, value);
        }

        public override int GetHashCode()
        {
            return (value != null ? value.GetHashCode() : 0);
        }

        public static bool operator ==(Polymorphism x, Polymorphism y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Polymorphism x, Polymorphism y)
        {
            return !(x == y);
        }

        public override string ToString()
        {
            return value;
        }

        public static Polymorphism FromString(string value)
        {
            return new Polymorphism(value);
        }
    }
}