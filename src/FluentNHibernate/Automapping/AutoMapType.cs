using System;

namespace FluentNHibernate.Automapping
{
    public class AutoMapType
    {
        public AutoMapType(Type type)
        {
            Type = type;
        }

        public Type Type { get; set;}
        public bool IsMapped { get; set; }
    }
}