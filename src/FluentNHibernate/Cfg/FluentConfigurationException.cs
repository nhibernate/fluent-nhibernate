using System;
using System.Collections.Generic;

namespace FluentNHibernate.Cfg
{
    public class FluentConfigurationException : Exception
    {
        public FluentConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
            PotentialReasons = new List<string>();
        }

        public IList<string> PotentialReasons { get; private set; }

        public override string Message
        {
            get
            {
                var output = base.Message;

                output += Environment.NewLine + Environment.NewLine;

                foreach (var reason in PotentialReasons)
                {
                    output += "  * " + reason;
                    output += Environment.NewLine;
                }

                return output;
            }
        }

        public override string ToString()
        {
            var output =  base.ToString();

            output += Environment.NewLine + Environment.NewLine;

            foreach (var reason in PotentialReasons)
            {
                output += "  * " + reason;
                output += Environment.NewLine;
            }

            return output;
        }
    }
}