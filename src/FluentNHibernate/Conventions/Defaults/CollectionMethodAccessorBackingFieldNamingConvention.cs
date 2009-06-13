using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Base convention for setting the backing field name of a property or method.
    /// </summary>
    public class CollectionMethodAccessorBackingFieldNamingConvention : ICollectionConvention
    {
        public void Accept(IAcceptanceCriteria<ICollectionInspector> acceptance)
        {
            acceptance.Expect(x => x.IsMethodAccess);
        }

        public void Apply(ICollectionAlteration alteration, ICollectionInspector inspector)
        {
            alteration.Name(inspector.Member.Name.Replace("Get", ""));
        }
    }
}