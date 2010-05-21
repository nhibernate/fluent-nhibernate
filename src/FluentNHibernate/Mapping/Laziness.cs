namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// Laziness strategy for relationships
    /// </summary>
    public class Laziness
    {
        /// <summary>
        /// No lazy loading
        /// </summary>
        public static readonly Laziness False = new Laziness("false");
            
        /// <summary>
        /// Proxy-based lazy-loading
        /// </summary>
        public static readonly Laziness Proxy = new Laziness("proxy");
            
        /// <summary>
        /// No proxy lazy loading
        /// </summary>
        public static readonly Laziness NoProxy = new Laziness("no-proxy");
            
        readonly string value;

        public Laziness(string value)
        {
            this.value = value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Laziness);
        }

        public bool Equals(Laziness other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.value, value);
        }

        public override int GetHashCode()
        {
            return (value != null ? value.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return value;
        }
    }
}