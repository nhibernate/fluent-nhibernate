using System.Reflection;

namespace FluentNHibernate.Mapping.Conventions.Defaults
{
    public abstract class BaseMethodAccessorBackingFieldNamingConvention<TPart>
        where TPart : ICollectionRelationship
    {
        public bool Accept(TPart target)
        {
            return target.IsMethodAccess;
        }

        public void Apply(TPart target, ConventionOverrides overrides)
        {
            if (overrides.GetMethodCollectionAccessorBackingFieldName == null)
                target.SetAttribute("name", target.Member.Name.Replace("Get", ""));
            else
                target.SetAttribute("name", overrides.GetMethodCollectionAccessorBackingFieldName(target.Member as MethodInfo));
        }
    }
}