namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// Naming strategy prefix.
    /// </summary>
    public class Prefix
    {
        public static readonly Prefix None = new Prefix("");
        public static readonly Prefix Underscore = new Prefix("-underscore");
        public static readonly Prefix m = new Prefix("-m");
        public static readonly Prefix mUnderscore = new Prefix("-m-underscore");

        private readonly string value;

        private Prefix(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get { return value; }
        }
    }
}