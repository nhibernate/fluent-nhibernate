namespace FluentNHibernate.Conventions.Inspections
{
    public class LowerCasePrefix : Prefix
    {
        public static readonly LowerCasePrefix None = new LowerCasePrefix("");
        public static readonly LowerCasePrefix Underscore = new LowerCasePrefix("-underscore");

        protected LowerCasePrefix(string value)
            : base(value)
        {}
    }
}