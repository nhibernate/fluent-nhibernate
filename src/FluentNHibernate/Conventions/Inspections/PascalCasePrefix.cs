namespace FluentNHibernate.Conventions.Inspections
{
    public class PascalCasePrefix : Prefix
    {
        public static readonly PascalCasePrefix M = new PascalCasePrefix("-m");
        public static readonly PascalCasePrefix Underscore = new PascalCasePrefix("-underscore");
        public static readonly PascalCasePrefix MUnderscore = new PascalCasePrefix("-m-underscore");

        protected PascalCasePrefix(string value)
            : base(value)
        { }
    }
}