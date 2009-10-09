using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class ProxyConventionTester
    {
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

        [Test]
        public void ConventionSetsProxyOnProxiedClass()
        {
            var convention = new ProxyConvention(PersistentTypeToProxy,
                                                 ProxyToPersistentType);

            var classInstance = MockRepository.GenerateMock<IClassInstance>();
            classInstance.Expect(x => x.EntityType).Return(typeof(ProxiedObject));
            
            convention.Apply(classInstance);

            classInstance.AssertWasCalled(x => x.Proxy(typeof(IProxiedObject)));
        }

        [Test]
        public void ConventionSetsProxyOnProxiedSubclass()
        {
            var convention = new ProxyConvention(PersistentTypeToProxy,
                                                 ProxyToPersistentType);

            var classInstance = MockRepository.GenerateMock<ISubclassInstance>();
            classInstance.Expect(x => x.EntityType).Return(typeof(ProxiedObject));

            convention.Apply(classInstance);

            classInstance.AssertWasCalled(x => x.Proxy(typeof(IProxiedObject)));
        }

        [Test]
        public void ConventionDoesNotSetProxyOnUnproxiedClass()
        {
            var convention = new ProxyConvention(PersistentTypeToProxy,
                                                 ProxyToPersistentType);

            var classInstance = MockRepository.GenerateMock<IClassInstance>();
            classInstance.Stub(x => x.EntityType).Return(typeof(NotProxied));

            convention.Apply(classInstance);

            classInstance.AssertWasNotCalled(x => x.Proxy(Arg<Type>.Is.Anything));
        }

        [Test]
        public void ConventionDoesNotSetProxyOnUnproxiedSubclass()
        {
            var convention = new ProxyConvention(PersistentTypeToProxy,
                                                 ProxyToPersistentType);

            var classInstance = MockRepository.GenerateMock<ISubclassInstance>();
            classInstance.Stub(x => x.EntityType).Return(typeof(NotProxied));

            convention.Apply(classInstance);

            classInstance.AssertWasNotCalled(x => x.Proxy(Arg<Type>.Is.Anything));
        }

        [Test]
        public void ConventionSetsProxiedCollectionChildTypeToConcreteType()
        {
            var convention = new ProxyConvention(PersistentTypeToProxy,
                                                 ProxyToPersistentType);

            var collectionInstance = MockRepository.GenerateMock<ICollectionInstance>();
            var relationship = MockRepository.GenerateMock<IRelationshipInstance>();

            collectionInstance.Stub(x => x.Relationship)
                .Return(relationship);
            relationship.Stub(x => x.Class)
                .Return(new TypeReference(typeof(IProxiedObject)));

            convention.Apply(collectionInstance);

            relationship.AssertWasCalled(x => x.CustomClass(typeof(ProxiedObject)));
        }

        [Test]
        public void ConventionDoesNotSetCollectionChildTypeIfUnrecognised()
        {
            var convention = new ProxyConvention(PersistentTypeToProxy,
                                                 ProxyToPersistentType);

            var collectionInstance = MockRepository.GenerateMock<ICollectionInstance>();
            var relationship = MockRepository.GenerateMock<IRelationshipInstance>();

            collectionInstance.Stub(x => x.Relationship)
                .Return(relationship);
            relationship.Stub(x => x.Class)
                .Return(new TypeReference(typeof(NotProxied)));

            convention.Apply(collectionInstance);

            relationship.AssertWasNotCalled(x => x.CustomClass(Arg<Type>.Is.Anything));
        }

        [Test]
        public void ConventionSetsProxiedManyToOneTypeToConcreteType()
        {
            var convention = new ProxyConvention(PersistentTypeToProxy,
                                                 ProxyToPersistentType);

            var manyToOneInstance = MockRepository.GenerateMock<IManyToOneInstance>();
            manyToOneInstance.Stub(x => x.Class)
                .Return(new TypeReference(typeof(IProxiedObject)));

            convention.Apply(manyToOneInstance);

            manyToOneInstance.AssertWasCalled(x => x.OverrideInferredClass(typeof(ProxiedObject)));
        }

        [Test]
        public void ConventionDoesNotSetManyToOneTypeIfUnrecognised()
        {
            var convention = new ProxyConvention(PersistentTypeToProxy,
                                                 ProxyToPersistentType);

            var mocks = new MockRepository();

            var manyToOne = mocks.DynamicMock<IManyToOneInstance>();

            using (mocks.Record())
            {
                manyToOne.Expect(x => x.Class)
                    .Return(new TypeReference(typeof(ProxiedObject)))
                    .Repeat.Any();
            }

            using (mocks.Playback())
            {
                convention.Apply(manyToOne);
            }
        }

        [Test]
        public void ConventionSetsProxiedOneToOneTypeToConcreteType()
        {
            var convention = new ProxyConvention(PersistentTypeToProxy,
                                                 ProxyToPersistentType);

            var mocks = new MockRepository();

            var oneToOne = mocks.DynamicMock<IOneToOneInstance>();

            using (mocks.Record())
            {
                oneToOne.Expect(x => ((IOneToOneInspector)x).Class)
                    .Return(new TypeReference(typeof(IProxiedObject)))
                    .Repeat.Any();

                oneToOne.OverrideInferredClass(typeof(ProxiedObject));
            }

            using (mocks.Playback())
            {
                convention.Apply(oneToOne);
            }
        }

        [Test]
        public void ConventionDoesNotSetOneToOneTypeIfUnrecognised()
        {
            var convention = new ProxyConvention(PersistentTypeToProxy,
                                                 ProxyToPersistentType);

            var mocks = new MockRepository();

            var manyToOne = mocks.DynamicMock<IOneToOneInstance>();

            using (mocks.Record())
            {
                manyToOne.Expect(x => ((IOneToOneInspector)x).Class)
                    .Return(new TypeReference(typeof(ProxiedObject)))
                    .Repeat.Any();
            }

            using (mocks.Playback())
            {
                convention.Apply(manyToOne);
            }
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
