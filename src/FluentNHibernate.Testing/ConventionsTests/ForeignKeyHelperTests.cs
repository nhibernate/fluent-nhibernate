using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.AutoMap.TestFixtures;
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
                .Element("class/many-to-one").HasAttribute("column", "ParentWoo");
        }

        [Test]
        public void ShouldUseFuncConventionForReferencesForeignKey()
        {
            new MappingTester<ExampleClass>()
                .Conventions(x => x.Add(ForeignKey.Format((p,t) => p.Name + "Woo")))
                .ForMapping(m => m.References(x => x.Parent))
                .Element("class/many-to-one").HasAttribute("column", "ParentWoo");
        }

        [Test]
        public void ShouldUseEndsWithConventionForHasManyForeignKey()
        {
            new MappingTester<OneToManyTarget>()
                .Conventions(x => x.Add(ForeignKey.EndsWith("Woo")))
                .ForMapping(m => m.HasMany(x => x.BagOfChildren))
                .Element("class/bag/key").HasAttribute("column", "OneToManyTargetWoo");
        }

        [Test]
        public void ShouldUseFuncConventionForHasManyForeignKey()
        {
            new MappingTester<OneToManyTarget>()
                .Conventions(x => x.Add(ForeignKey.Format((p, t) => t.Name + "Woo")))
                .ForMapping(m => m.HasMany(x => x.BagOfChildren))
                .Element("class/bag/key").HasAttribute("column", "OneToManyTargetWoo");
        }

        [Test]
        public void ShouldUseEndsWithConventionForHasManyToManyForeignKey()
        {
            new MappingTester<OneToManyTarget>()
                .Conventions(x => x.Add(ForeignKey.EndsWith("Woo")))
                .ForMapping(m => m.HasManyToMany(x => x.BagOfChildren))
                .Element("class/bag/key").HasAttribute("column", "OneToManyTargetWoo")
                .Element("class/bag/many-to-many").HasAttribute("column", "ChildObjectWoo");
        }

        [Test]
        public void ShouldUseFuncConventionForHasManyToManyForeignKey()
        {
            new MappingTester<OneToManyTarget>()
                .Conventions(x => x.Add(ForeignKey.Format((p, t) => t.Name + "Woo")))
                .ForMapping(m => m.HasManyToMany(x => x.BagOfChildren))
                .Element("class/bag/key").HasAttribute("column", "OneToManyTargetWoo")
                .Element("class/bag/many-to-many").HasAttribute("column", "ChildObjectWoo");
        }
    }
}
