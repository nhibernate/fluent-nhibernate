using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security;

namespace FluentNHibernate.Cfg
{
    [Serializable]
    public class FluentConfigurationException : Exception
    {
        public FluentConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
            PotentialReasons = new List<string>();
        }

        protected FluentConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.PotentialReasons = info.GetValue("PotentialReasons", typeof(List<string>)) as List<string>;            
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

        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("PotentialReasons", PotentialReasons, typeof(List<string>));
        }
    }

    
}