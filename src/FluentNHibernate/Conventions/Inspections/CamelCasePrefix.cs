namespace FluentNHibernate.Conventions.Inspections
{
    public class CamelCasePrefix : Prefix
    {
        public static readonly CamelCasePrefix None = new CamelCasePrefix("");
        public static readonly CamelCasePrefix Underscore = new CamelCasePrefix("-underscore");

        protected CamelCasePrefix(string value)
            : base(value)
        {}
    }
}