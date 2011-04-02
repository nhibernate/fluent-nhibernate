namespace FluentNHibernate.Mapping
{
    public class NamingStrategy
    {
        public static readonly NamingStrategy LowerCase = new NamingStrategy("lowercase");
        public static readonly NamingStrategy LowerCaseUnderscore = new NamingStrategy("lowercase-underscore");
        public static readonly NamingStrategy PascalCase = new NamingStrategy("pascalcase");
        public static readonly NamingStrategy PascalCaseM = new NamingStrategy("pascalcase-m");
        public static readonly NamingStrategy PascalCaseMUnderscore = new NamingStrategy("pascalcase-m-underscore");
        public static readonly NamingStrategy PascalCaseUnderscore = new NamingStrategy("pascalcase-underscore");
        public static readonly NamingStrategy CamelCase = new NamingStrategy("camelcase");
        public static readonly NamingStrategy CamelCaseUnderscore = new NamingStrategy("camelcase-underscore");
        public static readonly NamingStrategy Unknown = new NamingStrategy("[unknown]");

        readonly string strategy;

        NamingStrategy(string strategy)
        {
            this.strategy = strategy;
        }

        public override bool Equals(object obj)
        {
            if (obj is NamingStrategy)
                return Equals((NamingStrategy)obj);

            return base.Equals(obj);
        }

        public bool Equals(NamingStrategy other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return Equals(other.strategy, strategy);
        }

        public override int GetHashCode()
        {
            return (strategy != null ? strategy.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return strategy;
        }

        public static NamingStrategy FromString(string strategy)
        {
            return new NamingStrategy(strategy);
        }
    }
}