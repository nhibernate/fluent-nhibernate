using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using NHibernate;
using System.Collections;
using System.Drawing;

namespace FluentNHibernate.Testing.Testing
{
    [TestFixture]
    public class PersistenceSpecificationTester
    {
        public class Cat
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public Kitten FirstKitten { get; set; }
            public IList<Kitten> AllKittens { get; set; }
            public Bitmap Picture { get; set; }

            public Cat()
            {
                AllKittens = new List<Kitten>();
            }

            public IEnumerable<Kitten> EnumerableOfKittens { get { return AllKittens; } }
            public void AddKitten(Kitten kitten)
            {
                AllKittens.Add(kitten);
            }
        }

        public class Kitten
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }

        public class TestComparer : IEqualityComparer
        {
            bool IEqualityComparer.Equals(object x, object y)
            {
                if (x is Cat && y is Cat)
                    return ((Cat)x).Id == ((Cat)y).Id;
                if (x is Kitten && y is Kitten)
                    return ((Kitten)x).Id == ((Kitten)y).Id;

                return false;
            }

            public int GetHashCode(object obj)
            {
                if (obj is Cat)
                    return (int)((Cat)obj).Id;
                if (obj is Kitten)
                    return (int)((Kitten)obj).Id;

                return 0;
            }
        }

        public class DummyBitmapComparer : IEqualityComparer
        {
            bool IEqualityComparer.Equals(object x, object y)
            {
                return x is Bitmap && y is Bitmap;
            }

            public int GetHashCode(object obj)
            {
                return obj.GetHashCode();
            }
        }

        private PersistenceSpecification<Cat> spec;
        private ISession session;
        private ITransaction transaction;
        private Cat cat;
        private Cat identicalCat;
        private ISessionSource sessionSource;

        [SetUp]
        public void Setup()
        {
            var firstKitten = new Kitten { Id = 1, Name = "Kitten" };
            cat = new Cat
            {
                Id = 100,
                Name = "Cat",
                FirstKitten = firstKitten,
                Picture = new Bitmap(5, 5),
                AllKittens = new List<Kitten>
                {
                    firstKitten,
                    new Kitten {Id = 2, Name = "Kitten2"},
                }
            };

            firstKitten = new Kitten { Id = 1, Name = "IdenticalKitten" };
            identicalCat = new Cat
            {
                Id = 100,
                Name = "IdenticalCat",
                FirstKitten = firstKitten,
                Picture = new Bitmap(5, 5),
                AllKittens = new List<Kitten>
                {
                    firstKitten,
                    new Kitten {Id = 2, Name = "IdenticalKitten2"},
                }
            };

            transaction = MockRepository.GenerateStub<ITransaction>();

            session = MockRepository.GenerateStub<ISession>();
            session.Stub(s => s.BeginTransaction()).Return(transaction);
            session.Stub(s => s.Get<Cat>(null)).IgnoreArguments().Return(identicalCat);
            session.Stub(s => s.GetIdentifier(cat)).Return(cat.Id);

            sessionSource = MockRepository.GenerateStub<ISessionSource>();
            sessionSource.Stub(ss => ss.CreateSession()).Return(session);

            spec = new PersistenceSpecification<Cat>(sessionSource, new TestComparer());
        }

        [Test]
        public void Comparing_two_properties_should_use_the_specified_IEqualityComparer()
        {
            spec.CheckProperty(x => x.FirstKitten, cat.FirstKitten).VerifyTheMappings();
        }

        [Test]
        public void Comparing_objects_in_two_lists_should_use_the_specified_IEqualityComparer()
        {
            spec.CheckList(x => x.AllKittens, cat.AllKittens).VerifyTheMappings();
        }

        [Test]
        public void should_not_be_equal_without_the_equality_comparer()
        {
            spec = new PersistenceSpecification<Cat>(sessionSource);

            typeof(ApplicationException).ShouldBeThrownBy(() =>
                spec.CheckList(x => x.AllKittens, cat.AllKittens).VerifyTheMappings());
        }

        [Test]
        public void Comparing_objects_in_two_lists_should_use_the_specified_comparisons()
        {
            spec.CheckList(x => x.AllKittens, cat.AllKittens, kitten => kitten.Id).VerifyTheMappings();

            // Should fail because the names don't match.
            Assert.Throws<ApplicationException>(() => spec.CheckList(x => x.AllKittens, cat.AllKittens, kitten => kitten.Id, kitten => kitten.Name)
                .VerifyTheMappings());
        }

        [Test]
        public void Can_test_enumerable()
        {
            var kittens = new[] {new Kitten {Id = 3, Name = "kitten3"}, new Kitten {Id = 4, Name = "kitten4"}};
#pragma warning disable 618,612
            spec.CheckEnumerable(x => x.EnumerableOfKittens, (cat, kitten) => cat.AddKitten(kitten), kittens);
#pragma warning restore 618,612

            typeof(ApplicationException).ShouldBeThrownBy(() => spec.VerifyTheMappings());
        }

        [Test]
        public void Comparing_two_properties_should_use_the_specified_property_IEqualityComparer()
        {
            spec.CheckProperty(x => x.Picture, cat.Picture, new DummyBitmapComparer()).VerifyTheMappings ();
        }

    	[Test]
    	public void VerifyTheMappings_returns_instance()
    	{
			var cat = spec.CheckProperty(x => x.FirstKitten, this.cat.FirstKitten).VerifyTheMappings();
			cat.ShouldNotBeNull();
    	}

        [Test]
        public void Comparing_reference_should_use_the_specified_property_comparisons()
        {
            spec.CheckReference(x => x.FirstKitten, cat.FirstKitten, x => x.Id).VerifyTheMappings();

            // Should fail because the names don't match.
            Assert.Throws<ApplicationException>(() => spec.CheckReference(x => x.FirstKitten, cat.FirstKitten, x => x.Id, x => x.Name)
                .VerifyTheMappings());
        }
    }

    [TestFixture]
    public class PersistenceSpecificationConstructorTests
    {
        private ISessionSource sessionSource;

        [SetUp]
        public void Setup()
        {
            var transaction = MockRepository.GenerateStub<ITransaction>();
            var session = MockRepository.GenerateStub<ISession>();
            session.Stub(s => s.BeginTransaction()).Return(transaction);

            sessionSource = MockRepository.GenerateStub<ISessionSource>();
            sessionSource.Stub(ss => ss.CreateSession()).Return(session);
        }

        [Test]
        public void Should_accept_classes_with_public_parameterless_constructor()
        {
            var _spec = new PersistenceSpecification<PublicConstructorClass>(sessionSource);
            _spec.VerifyTheMappings();
        }

        [Test]
        public void Should_accept_classes_with_private_parameterless_constructor()
        {
            var _spec = new PersistenceSpecification<PrivateConstructorClass>(sessionSource);
            _spec.VerifyTheMappings();
        }

        [Test]
        public void Should_accept_classes_with_protected_parameterless_constructor()
        {
            var _spec = new PersistenceSpecification<ProtectedConstructorClass>(sessionSource);
            _spec.VerifyTheMappings();
        }

        [Test]
        public void Should_reject_classes_without_a_parameterless_constructor()
        {
            var _spec = new PersistenceSpecification<NoParameterlessConstructorClass>(sessionSource);

            typeof(MissingConstructorException).ShouldBeThrownBy(() =>
                _spec.VerifyTheMappings());
        }

        [Test]
        public void Should_accept_instances_regardless_of_constructor()
        {
            var _spec = new PersistenceSpecification<NoParameterlessConstructorClass>(sessionSource);
            _spec.VerifyTheMappings(new NoParameterlessConstructorClass(123));
        }

        public class PublicConstructorClass
        {
            public PublicConstructorClass() { }
        }

        public class ProtectedConstructorClass
        {
            protected ProtectedConstructorClass() { }
        }

        public class PrivateConstructorClass
        {
            private PrivateConstructorClass() { }
        }

        public class NoParameterlessConstructorClass
        {
            public NoParameterlessConstructorClass(int someParameter) { }
        }
    }
}