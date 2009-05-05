using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Base convention for setting the backing field name of a property or method.
    /// </summary>
    /// <typeparam name="TPart">Collection relationship mapping</typeparam>
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