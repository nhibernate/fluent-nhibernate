using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeItEasy;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class ProxyConventionTester
    {
        [Test]
        public void ConventionSetsProxyOnProxiedClass()
        {
            ProxyConvention convention = GetConvention();

            var classInstance = A.Fake<IClassInstance>();
            A.CallTo(() => classInstance.EntityType).Returns(typeof(ProxiedObject));

            convention.Apply(classInstance);

            A.CallTo(() => classInstance.Proxy(typeof(IProxiedObject)))
                .MustHaveHappened();
        }

        [Test]
        public void ConventionSetsProxyOnProxiedSubclass()
        {
            ProxyConvention convention = GetConvention();

            var classInstance = A.Fake<ISubclassInstance>();
            A.CallTo(() => classInstance.EntityType).Returns(typeof(ProxiedObject));

            convention.Apply(classInstance);

            A.CallTo(() => classInstance.Proxy(typeof(IProxiedObject)))
                .MustHaveHappened();
        }

        [Test]
        public void ConventionDoesNotSetProxyOnUnproxiedClass()
        {
            ProxyConvention convention = GetConvention();

            var classInstance = A.Fake<IClassInstance>();
            A.CallTo(() => classInstance.EntityType).Returns(typeof(NotProxied));

            convention.Apply(classInstance);

            A.CallTo(() => classInstance.Proxy(A<Type>._)).MustNotHaveHappened();
        }

        [Test]
        public void ConventionDoesNotSetProxyOnUnproxiedSubclass()
        {
            ProxyConvention convention = GetConvention();

            var classInstance = A.Fake<ISubclassInstance>();
            A.CallTo(() => classInstance.EntityType).Returns(typeof(NotProxied));

            convention.Apply(classInstance);

            A.CallTo(() => classInstance.Proxy(A<Type>._)).MustNotHaveHappened();
        }

        [Test]
        public void ConventionSetsProxiedCollectionChildTypeToConcreteType()
        {
            ProxyConvention convention = GetConvention();

            var collectionInstance = A.Fake<ICollectionInstance>();
            var relationship = A.Fake<IRelationshipInstance>();

            A.CallTo(() => collectionInstance.Relationship).Returns(relationship);
            A.CallTo(() => relationship.Class).Returns(new TypeReference(typeof(IProxiedObject)));

            convention.Apply(collectionInstance);

            A.CallTo(() => relationship.CustomClass(typeof(ProxiedObject)))
                .MustHaveHappened();
        }

        [Test]
        public void ConventionDoesNotSetCollectionChildTypeIfUnrecognised()
        {
            ProxyConvention convention = GetConvention();

            var collectionInstance = A.Fake<ICollectionInstance>();
            var relationship = A.Fake<IRelationshipInstance>();

            A.CallTo(() => collectionInstance.Relationship).Returns(relationship);
            A.CallTo(() => relationship.Class).Returns(new TypeReference(typeof(NotProxied)));

            convention.Apply(collectionInstance);

            A.CallTo(() => relationship.CustomClass(A<Type>._))
                .MustNotHaveHappened();
        }

        [Test]
        public void ConventionSetsProxiedManyToOneTypeToConcreteType()
        {
            ProxyConvention convention = GetConvention();

            var manyToOneInstance = A.Fake<IManyToOneInstance>();
            A.CallTo(() => manyToOneInstance.Class).Returns(new TypeReference(typeof(IProxiedObject)));

            convention.Apply(manyToOneInstance);

            A.CallTo(() => manyToOneInstance.OverrideInferredClass(typeof(ProxiedObject)))
                .MustHaveHappened();
        }

        [Test]
        public void ConventionDoesNotSetManyToOneTypeIfUnrecognised()
        {
            ProxyConvention convention = GetConvention();

            var manyToOneInstance = A.Fake<IManyToOneInstance>();
            A.CallTo(() => manyToOneInstance.Class).Returns(new TypeReference(typeof(NotProxied)));

            convention.Apply(manyToOneInstance);

            A.CallTo(() => manyToOneInstance.OverrideInferredClass(typeof(ProxiedObject)))
                .MustNotHaveHappened();
        }

        [Test]
        public void ConventionSetsProxiedOneToOneTypeToConcreteType()
        {
            ProxyConvention convention = GetConvention();

            var oneToOneInstance = A.Fake<IOneToOneInstance>();
            A.CallTo(() => ((IOneToOneInspector)oneToOneInstance).Class)
                .Returns(new TypeReference(typeof(IProxiedObject)));

            convention.Apply(oneToOneInstance);

            A.CallTo(() => oneToOneInstance.OverrideInferredClass(typeof(ProxiedObject)))
                .MustHaveHappened();
        }

        [Test]
        public void ConventionDoesNotSetOneToOneTypeIfUnrecognised()
        {
            ProxyConvention convention = GetConvention();

            var oneToOneInstance = A.Fake<IOneToOneInstance>();
            A.CallTo(() => ((IOneToOneInspector)oneToOneInstance).Class)
                .Returns(new TypeReference(typeof(NotProxied)));

            convention.Apply(oneToOneInstance);

            A.CallTo(() => oneToOneInstance.OverrideInferredClass(typeof(ProxiedObject)))
                .MustNotHaveHappened();
        }

        private static ProxyConvention GetConvention()
        {
            return new ProxyConvention(PersistentTypeToProxy,
                ProxyToPersistentType);
        }

        private static Type PersistentTypeToProxy(Type type)
        {
            return type == typeof(ProxiedObject)
                           ? typeof(IProxiedObject)
                           : null;
        }

        private static Type ProxyToPersistentType(Type type)
        {
            return type == typeof(IProxiedObject)
                           ? typeof(ProxiedObject)
                           : null;
        }

        public interface IProxiedObject
        {
            int Id { get; set; }
            
        }

        public class ProxiedObject : IProxiedObject
        {
            public int Id { get; set; }
            
        }

        public class NotProxied{}

        public interface IProxiedSubclass : IProxiedObject
        {
            
        }

        public class ProxiedSubclass: ProxiedObject, IProxiedSubclass
        {
            
        }

        public class NotProxiedSubclass : NotProxied
        {
            
        }
    }
}
