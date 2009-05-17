using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
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

        //[Test]
        //public void CanAddType()
        //{
        //    collection.Add(new DummyAssemblyConvention());
        //    collection.Contains(typeof(DummyAssemblyConvention))
        //        .ShouldBeTrue();
        //}

        //[Test]
        //public void CanAddInstanceOfType()
        //{
        //    collection.Add(new DummyAssemblyConvention());
        //    collection[typeof(DummyAssemblyConvention)]
        //        .ShouldNotBeNull();
        //}

        //[Test]
        //public void CanAddMultipleInstances()
        //{
        //    collection.Add(new DummyAssemblyConvention());
        //    collection.Add(new DummyAssemblyConvention());
        //    collection[typeof(DummyAssemblyConvention)]
        //        .ShouldHaveCount(2);
        //}

        //[Test]
        //public void CanIterateCollection()
        //{
        //    collection.Add(new DummyAssemblyConvention());
        //    collection.Add(new MultiPartConvention());

        //    var oneFound = false;
        //    var twoFound = false;

        //    foreach (var type in collection)
        //    {
        //        if (type == typeof(DummyAssemblyConvention))
        //            oneFound = true;
        //        if (type == typeof(MultiPartConvention))
        //            twoFound = true;
        //    }
            
        //    oneFound.ShouldBeTrue();
        //    twoFound.ShouldBeTrue();
        //}
    }
}
