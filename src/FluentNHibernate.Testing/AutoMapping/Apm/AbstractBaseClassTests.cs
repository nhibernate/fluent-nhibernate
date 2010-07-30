using System;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.MappingModel.Identity;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Automapping.Apm
{
    [TestFixture]
    public class AbstractBaseClassTests
    {
        [Test]
        public void ShouldBeConsideredLayerSuperTypesByDefault()
        {
            var automapper =
                AutoMap.Source(new StubTypeSource(new[] { typeof(AbstractBase), typeof(Child) }));
            automapper.ValidationEnabled = false;
            var mappings = automapper.BuildMappings();

            mappings
                .SelectMany(x => x.Classes)
                .ShouldNotContain(x => x.Type == typeof(AbstractBase));
        }

        [Test]
        public void ShouldAllowSpecifyingToNotTreatAbstractsAsLayerSuperTypes()
        {
            var cfg = new TestConfiguration_AbstractClassIsNeverLayerSupertype();
            var automapper = AutoMap.Source(new StubTypeSource(new[] { typeof(AbstractBase), typeof(Child) }), cfg);

            automapper.ValidationEnabled = false;
            var mappings = automapper.BuildMappings();

            mappings
                .SelectMany(x => x.Classes)
                .ShouldContain(x => x.Type == typeof(AbstractBase));
        }

        [Test]
        public void ShouldAllowExplicitInclusionOfAnAbstractClass()
        {
            var automapper =
                AutoMap.Source(new StubTypeSource(new[] { typeof(AbstractBase), typeof(Child) }))
                    .IncludeBase<AbstractBase>();

            automapper.ValidationEnabled = false;
            var mappings = automapper.BuildMappings();

            mappings
                .SelectMany(x => x.Classes)
                .ShouldContain(x => x.Type == typeof(AbstractBase));
        }

        class TestConfiguration_AbstractClassIsNeverLayerSupertype : DefaultAutomappingConfiguration
        {
            public override bool AbstractClassIsLayerSupertype(Type type)
            {
                return false;
            }
        }
    }

    public abstract class AbstractBase
    {
        
    }

    public class Child : AbstractBase
    {
        
    }
}