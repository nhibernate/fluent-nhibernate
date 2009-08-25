using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using NHibernate;
using System.Collections;

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
        }

        public class Kitten
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }

        public class TestComparer : IEqualityComparer
        {
            public new bool Equals(object x, object y)
            {
                if (x is Cat && y is Cat)
                    return ((Cat)x).Id == ((Cat)y).Id;
                else if (x is Kitten && y is Kitten)
                    return ((Kitten)x).Id == ((Kitten)y).Id;
                throw new System.NotImplementedException();
            }

            public int GetHashCode(object obj)
            {
                if (obj is Cat)
                    return (int) ((Cat) obj).Id;
                else if (obj is Kitten)
                    return (int) ((Kitten)obj).Id;
                throw new NotImplementedException();
            }
        }

        private PersistenceSpecification<Cat> _spec;
        private ISession _session;
        private ITransaction _transaction;
        private Cat _cat;
        private Cat _identicalCat;
        private ISessionSource _sessionSource;

        [SetUp]
        public void Setup()
        {
            var firstKitten = new Kitten { Id = 1, Name = "Kitten" };
            _cat = new Cat
            {
                Id = 100,
                Name = "Cat",
                FirstKitten = firstKitten,
                AllKittens = new List<Kitten>
                {
                    firstKitten,
                    new Kitten {Id = 2, Name = "Kitten2"},
                }
            };

            firstKitten = new Kitten { Id = 1, Name = "IdenticalKitten" };
            _identicalCat = new Cat
            {
                Id = 100,
                Name = "IdenticalCat",
                FirstKitten = firstKitten,
                AllKittens = new List<Kitten>
                {
                    firstKitten,
                    new Kitten {Id = 2, Name = "IdenticalKitten2"},
                }
            };

            _transaction = MockRepository.GenerateStub<ITransaction>();

            _session = MockRepository.GenerateStub<ISession>();
            _session.Stub(s => s.BeginTransaction()).Return(_transaction);
            _session.Stub(s => s.Get<Cat>(null)).IgnoreArguments().Return(_identicalCat);
            _session.Stub(s => s.GetIdentifier(_cat)).Return(_cat.Id);

            _sessionSource = MockRepository.GenerateStub<ISessionSource>();
            _sessionSource.Stub(ss => ss.CreateSession()).Return(_session);

            _spec = new PersistenceSpecification<Cat>(_sessionSource, new TestComparer());
        }

        [Test]
        public void Comparing_two_properties_should_use_the_specified_IEqualityComparer()
        {
            _spec.CheckProperty(x => x.FirstKitten, _cat.FirstKitten).VerifyTheMappings();
        }

        [Test]
        public void Comparing_objects_in_two_lists_should_use_the_specified_IEqualityComparer()
        {
            _spec.CheckList(x => x.AllKittens, _cat.AllKittens).VerifyTheMappings();
        }

        [Test]
        public void should_not_be_equal_without_the_equality_comparer()
        {
            _spec = new PersistenceSpecification<Cat>(_sessionSource);

            typeof (ApplicationException).ShouldBeThrownBy(() => 
                _spec.CheckList(x => x.AllKittens, _cat.AllKittens).VerifyTheMappings());
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

        public class PublicConstructorClass 
        {
            public PublicConstructorClass() {}
        }

        public class ProtectedConstructorClass
        {
            protected ProtectedConstructorClass() {}
        }

        public class PrivateConstructorClass
        {
            private PrivateConstructorClass() { }
        }

        public class NoParameterlessConstructorClass
        {
            public NoParameterlessConstructorClass(int someParameter) {}
        }
    }
}