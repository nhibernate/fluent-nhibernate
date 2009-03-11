using System.Reflection;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Defaults
{
    public abstract class BaseMethodAccessorBackingFieldNamingConvention<TPart>
        where TPart : ICollectionRelationship
    {
        public bool Accept(TPart target)
        {
            return target.IsMethodAccess;
        }

        public void Apply(TPart target)
        {
            target.SetAttribute("name", target.Member.Name.Replace("Get", ""));
        }
    }
}