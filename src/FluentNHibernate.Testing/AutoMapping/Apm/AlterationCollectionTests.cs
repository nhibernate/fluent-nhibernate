using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Testing.Fixtures.AutoMappingAlterations;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Automapping.Apm
{
    [TestFixture]
    public class AlterationCollectionTests
    {
        private AutoMappingAlterationCollection alterations;

        [SetUp]
        public void CreateAlterationsContainer()
        {
            alterations = new AutoMappingAlterationCollection();
        }

        [Test]
        public void ShouldBeAbleToAddAllAlterationsFromAssembly()
        {
            alterations.AddFromAssembly(typeof(DummyAlteration1).Assembly);

            alterations.ShouldContain(a => a is DummyAlteration1);
            alterations.ShouldContain(a => a is DummyAlteration2);
        }

        [Test]
        public void ShouldBeAbleToAddAllAlterationsFromAssemblyByType()
        {
            alterations.AddFromAssemblyOf<DummyAlteration1>();

            alterations.ShouldContain(a => a is DummyAlteration1);
            alterations.ShouldContain(a => a is DummyAlteration2);
        }

        [Test]
        public void ShouldBeAbleToAddSingleAlteration()
        {
            alterations.Add(new DummyAlteration1());

            alterations.ShouldContain(a => a is DummyAlteration1);
        }

        [Test]
        public void ShouldBeAbleToAddSingleAlterationByType()
        {
            alterations.Add<DummyAlteration1>();

            alterations.ShouldContain(a => a is DummyAlteration1);
        }

        [Test]
        public void ShouldntAddAlterationIfAddedAlready()
        {
            alterations.AddFromAssemblyOf<DummyAlteration1>();

            alterations.ShouldContain(a => a is DummyAlteration1);

            var originalCount = alterations.Count();

            alterations.Add<DummyAlteration1>();

            (alterations.Count() == originalCount).ShouldBeTrue();
        }
    }
}