using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Testing.ConventionFinderTests;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class ConventionsCollectionTests
    {
        private ConventionsCollection collection;

        [SetUp]
        public void CreateCollection()
        {
            collection = new ConventionsCollection();
        }

        [Test]
        public void CanAddType()
        {
            collection.Add(new DummyClassConvention());
            collection.Contains(typeof(DummyClassConvention))
                .ShouldBeTrue();
        }

        [Test]
        public void CanAddInstanceOfType()
        {
            collection.Add(new DummyClassConvention());
            collection[typeof(DummyClassConvention)]
                .ShouldNotBeNull();
        }

        [Test]
        public void CanAddMultipleInstances()
        {
            collection.Add(new DummyClassConvention());
            collection.Add(new DummyClassConvention());
            collection[typeof(DummyClassConvention)]
                .ShouldHaveCount(2);
        }

        [Test]
        public void CanIterateCollection()
        {
            collection.Add(new DummyClassConvention());
            collection.Add(new MultiPartConvention());

            var oneFound = false;
            var twoFound = false;

            foreach (var type in collection)
            {
                if (type == typeof(DummyClassConvention))
                    oneFound = true;
                if (type == typeof(MultiPartConvention))
                    twoFound = true;
            }

            oneFound.ShouldBeTrue();
            twoFound.ShouldBeTrue();
        }
    }
}
