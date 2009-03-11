namespace FluentNHibernate.Mapping.Conventions
{
    public class DefaultIdConvention : IMappingPartConvention
    {
        private readonly IConventionFinder conventionFinder;

        public DefaultIdConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IMappingPart part)
        {
            return (part is IIdentityPart);
        }

        public void Apply(IMappingPart part, ConventionOverrides overrides)
        {
            var conventions = conventionFinder.Find<IIdConvention>();
            var id = (IIdentityPart)part;

            foreach (var convention in conventions)
            {
                if (convention.Accept(id))
                    convention.Apply(id, overrides);
            }
        }
    }
}