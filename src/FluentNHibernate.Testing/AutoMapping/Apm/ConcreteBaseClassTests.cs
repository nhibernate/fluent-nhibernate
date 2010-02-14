using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.Testing.Automapping.Apm;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Apm
{
    [TestFixture]
    public class ConcreteBaseClassTests
    {
        [Test]
        public void ShouldAllowPropertiesWithSameNameToExistInDerivedClasses()
        {
            var automapper =
                AutoMap.Source(new StubTypeSource(new[] { typeof(BaseDomain), typeof(Subclass1), typeof(Subclass2), typeof(Subclass3) }));

            automapper.MergeMappings = true;
            var mappings = automapper.BuildMappings();

            mappings
                .SelectMany(x => x.Classes)
                .SelectMany(x => x.Subclasses)
                .SelectMany(c => c.Properties)
                .Count(p => p.Name == "CommonField1")
                .ShouldEqual(2);
        }
    }

    public class BaseDomain
    {
        public long Id { get; set; }
    }

    public class Subclass1 : BaseDomain
    {
        public string CommonField1 { get; set; }
    }

    public class Subclass2 : BaseDomain
    {
        public string CommonField1 { get; set; }
    }

    public class Subclass3 : BaseDomain
    {

    }
}
