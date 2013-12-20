using System;
using System.Linq.Expressions;
using FakeItEasy;
using FluentNHibernate.Testing.Values;
using FluentNHibernate.Utils.Reflection;
using NHibernate;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Testing.Values
{
    [TestFixture]
    public class When_a_reference_property_is_registered_on_the_persistence_specification : Specification
    {
        private ReferenceProperty<PropertyEntity, OtherEntity> sut;
        private PersistenceSpecification<PropertyEntity> specification;
        private OtherEntity referencedEntity;
        private ISession session;

        public override void establish_context()
        {
            var property = ReflectionHelper.GetAccessor((Expression<Func<ReferenceEntity, OtherEntity>>)(x => x.Reference));

            referencedEntity = new OtherEntity();

            session = A.Fake<ISession>();
            A.CallTo(() => session.BeginTransaction()).Returns(A.Dummy<ITransaction>());
            specification = new PersistenceSpecification<PropertyEntity>(session);

            sut = new ReferenceProperty<PropertyEntity, OtherEntity>(property, referencedEntity);
        }

        public override void because()
        {
            sut.HasRegistered(specification);
        }

        [Test]
        public void should_save_the_referenced_entity()
        {
            A.CallTo(() => session.Save(referencedEntity))
                .MustHaveHappened();
        }
    }
}