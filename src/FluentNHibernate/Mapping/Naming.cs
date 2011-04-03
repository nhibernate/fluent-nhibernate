using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentNHibernate.Mapping
{
    public static class Naming
    {
        static readonly List<string> Invalids = new List<string>
        {
            "camelcase-m",
            "camelcase-m-underscore",
            "lowercase-m",
            "lowercase-m-underscore",
        };

        public static NamingStrategy Determine(string name)
        {
            var strategy = GuessNamingStrategy(name);

            if (Invalids.Contains(strategy))
                return NamingStrategy.Unknown;

            return NamingStrategy.FromString(strategy);
        }

        static string GuessNamingStrategy(string name)
        {
            if (name.StartsWith("_"))
                return GuessNamingStrategy(name.Substring(1)) + "-underscore";
            if (name.StartsWith("m_"))
                return GuessNamingStrategy(name.Substring(2)) + "-m-underscore";
            if (name.StartsWith("m") && char.IsUpper(name[1]))
                return GuessNamingStrategy(name.Substring(1)) + "-m";

            if (name.All(char.IsLower))
                return "lowercase";
            if (char.IsUpper(name[0]))
                return "pascalcase";

            return "camelcase";
        }
    }
}