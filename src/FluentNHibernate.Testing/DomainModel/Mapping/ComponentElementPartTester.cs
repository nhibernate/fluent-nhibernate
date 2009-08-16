using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ComponentElementPartTester
    {
        [Test]
        public void CanIncludeParentReference()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                    m.HasMany(x => x.Components)
                        .Component(c =>
                        {
                            c.Map(x => x.Name);
                            c.ParentReference(x => x.MyParent);
                        }))
                .Element("class/bag/composite-element/parent").Exists()
                .HasAttribute("name", "MyParent");
        }
    }
}