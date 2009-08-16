using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Testing.DomainModel.Mapping;
using FluentNHibernate.Testing.Fixtures.Basic;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class ForeignKeyHelperTests
    {
        [Test]
        public void ShouldUseEndsWithConventionForReferencesForeignKey()
        {
            new MappingTester<ExampleClass>()
                .Conventions(x => x.Add(ForeignKey.EndsWith("Woo")))
                .ForMapping(m => m.References(x => x.Parent))
                .Element("class/many-to-one/column").HasAttribute("name", "ParentWoo");
        }

        [Test]
        public void ShouldUseFuncConventionForReferencesForeignKey()
        {
            new MappingTester<ExampleClass>()
                .Conventions(x => x.Add(ForeignKey.Format((p,t) => p.Name + "Woo")))
                .ForMapping(m => m.References(x => x.Parent))
                .Element("class/many-to-one/column").HasAttribute("name", "ParentWoo");
        }

        [Test]
        public void ShouldUseEndsWithConventionForHasManyForeignKey()
        {
            new MappingTester<OneToManyTarget>()
                .Conventions(x => x.Add(ForeignKey.EndsWith("Woo")))
                .ForMapping(m => m.HasMany(x => x.BagOfChildren))
                .Element("class/bag/key/column").HasAttribute("name", "OneToManyTargetWoo");
        }

        [Test]
        public void ShouldUseFuncConventionForHasManyForeignKey()
        {
            new MappingTester<OneToManyTarget>()
                .Conventions(x => x.Add(ForeignKey.Format((p, t) => t.Name + "Woo")))
                .ForMapping(m => m.HasMany(x => x.BagOfChildren))
                .Element("class/bag/key/column").HasAttribute("name", "OneToManyTargetWoo");
        }

        [Test]
        public void ShouldUseEndsWithConventionForHasManyToManyForeignKey()
        {
            new MappingTester<OneToManyTarget>()
                .Conventions(x => x.Add(ForeignKey.EndsWith("Woo")))
                .ForMapping(m => m.HasManyToMany(x => x.BagOfChildren))
                .Element("class/bag/key/column").HasAttribute("name", "OneToManyTargetWoo")
                .Element("class/bag/many-to-many/column").HasAttribute("name", "ChildObjectWoo");
        }

        [Test]
        public void ShouldUseFuncConventionForHasManyToManyForeignKey()
        {
            new MappingTester<OneToManyTarget>()
                .Conventions(x => x.Add(ForeignKey.Format((p, t) => t.Name + "Woo")))
                .ForMapping(m => m.HasManyToMany(x => x.BagOfChildren))
                .Element("class/bag/key/column").HasAttribute("name", "OneToManyTargetWoo")
                .Element("class/bag/many-to-many/column").HasAttribute("name", "ChildObjectWoo");
        }
    }
}
