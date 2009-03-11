using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.UserAlterations
{
    public class ReferencesConvention : IReferenceConvention
    {
        public bool Accept(IManyToOnePart target)
        {
            return true;
        }

        public void Apply(IManyToOnePart target, ConventionOverrides overrides)
        {
            if (overrides.ManyToOneConvention != null)
                overrides.ManyToOneConvention(target);
        }
    }
}