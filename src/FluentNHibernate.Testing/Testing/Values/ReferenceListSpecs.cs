using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FakeItEasy;
using FluentNHibernate.Testing.Values;
using FluentNHibernate.Utils.Reflection;
using NHibernate;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Testing.Values
{
    [TestFixture]
    public class When_a_reference_list_is_registered_on_the_persistence_specification : Specification
    {
        private ReferenceList<PropertyEntity, OtherEntity> sut;
        private PersistenceSpecification<PropertyEntity> specification;
        private ISession session;
        private List<OtherEntity> referencedEntities;

        public override void establish_context()
        {
            var property = ReflectionHelper.GetAccessor((Expression<Func<ReferenceEntity, object>>)(x => x.ReferenceList));

            referencedEntities = new List<OtherEntity> { new OtherEntity(), new OtherEntity() };

            session = A.Fake<ISession>();
            A.CallTo(() => session.BeginTransaction()).Returns(A.Dummy<ITransaction>());
            specification = new PersistenceSpecification<PropertyEntity>(session);

            sut = new ReferenceList<PropertyEntity, OtherEntity>(property, referencedEntities);
        }

        public override void because()
        {
            sut.HasRegistered(specification);
        }

        [Test]
        public void should_save_the_referenced_list_items()
        {
            foreach (var reference in referencedEntities)
            {
                OtherEntity entity = reference;
                A.CallTo(() => session.Save(entity))
                    .MustHaveHappened();
            }
        }
    }
}