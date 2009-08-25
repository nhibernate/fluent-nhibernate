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

            automapper.CompileMappings();
            var mappings = automapper.BuildMappings();

            mappings
                .SelectMany(x => x.Classes)
                .ShouldNotContain(x => x.Type == typeof(AbstractBase));
        }

        [Test]
        public void ShouldAllowSpecifyingToNotTreatAbstractsAsLayerSuperTypes()
        {
            var automapper =
                AutoMap.Source(new StubTypeSource(new[] { typeof(AbstractBase), typeof(Child) }))
                    .Setup(x => x.AbstractClassIsLayerSupertype = t => false);

            automapper.CompileMappings();
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

            automapper.CompileMappings();
            var mappings = automapper.BuildMappings();

            mappings
                .SelectMany(x => x.Classes)
                .ShouldContain(x => x.Type == typeof(AbstractBase));
        }
    }

    public abstract class AbstractBase
    {
        
    }

    public class Child : AbstractBase
    {
        
    }
}