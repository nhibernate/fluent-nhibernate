using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.Alterations;
using FluentNHibernate.Testing.Fixtures.AutoMappingAlterations;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMap.Apm
{
    [TestFixture]
    public class AlterationContainerTests
    {
        private ExposedAlterationContainer alterations;

        [SetUp]
        public void CreateAlterationsContainer()
        {
            alterations = new ExposedAlterationContainer();
        }

        [Test]
        public void ShouldBeAbleToAddAllAlterationsFromAssembly()
        {
            alterations.AddFromAssembly(typeof(DummyAlteration1).Assembly);

            alterations.Added.ShouldContain(a => a is DummyAlteration1);
            alterations.Added.ShouldContain(a => a is DummyAlteration2);
        }

        [Test]
        public void ShouldBeAbleToAddAllAlterationsFromAssemblyByType()
        {
            alterations.AddFromAssemblyOf<DummyAlteration1>();

            alterations.Added.ShouldContain(a => a is DummyAlteration1);
            alterations.Added.ShouldContain(a => a is DummyAlteration2);
        }

        [Test]
        public void ShouldBeAbleToAddSingleAlteration()
        {
            alterations.Add(new DummyAlteration1());

            alterations.Added.ShouldContain(a => a is DummyAlteration1);
        }

        [Test]
        public void ShouldBeAbleToAddSingleAlterationByType()
        {
            alterations.Add<DummyAlteration1>();

            alterations.Added.ShouldContain(a => a is DummyAlteration1);
        }

        [Test]
        public void ShouldntAddAlterationIfAddedAlready()
        {
            alterations.AddFromAssemblyOf<DummyAlteration1>();

            alterations.Added.ShouldContain(a => a is DummyAlteration1);

            var originalCount = alterations.Added.Count();

            alterations.Add<DummyAlteration1>();

            (alterations.Added.Count() == originalCount).ShouldBeTrue();
        }

        private class ExposedAlterationContainer : AutoMappingAlterationContainer
        {
            public IEnumerable<IAutoMappingAlteration> Added
            {
                get { return Alterations; }
            }
        }
    }
}