namespace FluentNHibernate.Mapping.Conventions
{
    public class DefaultJoinConvention : IMappingPartConvention
    {
        private readonly IConventionFinder conventionFinder;

        public DefaultJoinConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IMappingPart target)
        {
            return (target is IJoin);
        }

        public void Apply(IMappingPart target, ConventionOverrides overrides)
        {
            var conventions = conventionFinder.Find<IJoinConvention>();
            var join = (IJoin)target;

            foreach (var convention in conventions)
            {
                if (convention.Accept(join))
                    convention.Apply(join, overrides);
            }
        }
    }
}