using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Examples.FirstAutomappedProject
{
    /// <summary>
    /// This is a convention that will be applied to all entities in your application. What this particular
    /// convention does is to specify that many-to-one, one-to-many, and many-to-many relationships will all
    /// have their Cascade option set to All.
    /// </summary>
    class CascadeConvention : IReferenceConvention, IHasManyConvention, IHasManyToManyConvention
    {
        public void Apply(IManyToOneInstance instance)
        {
            instance.Cascade.All();
        }

        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Cascade.All();
        }

        public void Apply(IManyToManyCollectionInstance instance)
        {
            instance.Cascade.All();
        }
    }
}