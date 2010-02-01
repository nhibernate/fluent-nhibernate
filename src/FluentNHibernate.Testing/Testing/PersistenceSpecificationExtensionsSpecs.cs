using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Testing.Testing.Values;
using FluentNHibernate.Testing.Values;
using NHibernate;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.Testing
{
    public class InspectablePersistenceSpecification<T> : PersistenceSpecification<T>
    {
        public InspectablePersistenceSpecification(ISessionSource source) : base(source)
        {}

        public InspectablePersistenceSpecification(ISessionSource source, IEqualityComparer entityEqualityComparer) : base(source, entityEqualityComparer)
        {}

        public InspectablePersistenceSpecification(ISession session) : base(session)
        {}

        public InspectablePersistenceSpecification(ISession session, IEqualityComparer entityEqualityComparer) : base(session, entityEqualityComparer)
        {}

        public List<Property<T>> AllProperties
        {
            get { return allProperties; }
        }
    }

    public abstract class With_persistence_specification<T> : Specification
    {
        private ISession session;
        protected InspectablePersistenceSpecification<T> sut;
        protected IEqualityComparer comparer;

        public override void establish_context()
        {
            session = MockRepository.GenerateStub<ISession>();
            session.Stub(x => x.BeginTransaction()).Return(MockRepository.GenerateStub<ITransaction>());

            comparer = MockRepository.GenerateStub<IEqualityComparer>();
            sut = new InspectablePersistenceSpecification<T>(session, comparer);
        }
    }

    [TestFixture]
    public class When_a_checked_property_is_added : With_persistence_specification<PropertyEntity>
    {
        public override void because()
        {
            sut.CheckProperty(x => x.GetterAndSetter, "expected");
        }

        [Test]
        public void should_add_a_property_check()
        {
            sut.AllProperties.First().ShouldBeOfType(typeof(Property<PropertyEntity, object>));
        }

        [Test]
        public void should_add_one_check_to_the_specification()
        {
            sut.AllProperties.ShouldHaveCount(1);
        }

        [Test]
        public void should_set_the_custom_equality_comparer()
        {
            sut.AllProperties.First().EntityEqualityComparer.ShouldEqual(comparer);
        }
    }

    [TestFixture]
    public class When_a_checked_property_of_an_array_type_is_added : With_persistence_specification<ListEntity>
    {
        public override void because()
        {
            sut.CheckProperty(x => x.Array, new[] {"foo", "bar", "baz"});
        }

        [Test]
        public void should_add_a_list_check()
        {
            sut.AllProperties.First().ShouldBeOfType(typeof(List<ListEntity, string>));
        }

        [Test]
        public void should_add_one_check_to_the_specification()
        {
            sut.AllProperties.ShouldHaveCount(1);
        }

        [Test]
        public void should_set_the_custom_equality_comparer()
        {
            sut.AllProperties.First().EntityEqualityComparer.ShouldEqual(comparer);
        }
    }

    [TestFixture]
    public class When_a_checked_property_with_a_custom_setter_is_added : With_persistence_specification<PropertyEntity>
    {
        protected Action<PropertyEntity, string> propertySetter;

        public override void establish_context()
        {
            base.establish_context();
            propertySetter = MockRepository.GenerateStub<Action<PropertyEntity, string>>();
        }

        public override void because()
        {
            sut.CheckProperty(x => x.GetterAndSetter, "expected", propertySetter);
        }

        [Test]
        public void should_add_a_property_check()
        {
            sut.AllProperties.First().ShouldBeOfType(typeof(Property<PropertyEntity, string>));
        }

        [Test]
        public void should_add_one_check_to_the_specification()
        {
            sut.AllProperties.ShouldHaveCount(1);
        }

        [Test]
        public void should_set_the_custom_equality_comparer()
        {
            sut.AllProperties.First().EntityEqualityComparer.ShouldEqual(comparer);
        }
    }

    [TestFixture]
    public class When_the_value_setter_of_a_checked_property_is_invoked : When_a_checked_property_with_a_custom_setter_is_added
    {
        private PropertyEntity entity;

        public override void establish_context()
        {
            base.establish_context();
            entity = new PropertyEntity();
        }

        public override void because()
        {
            base.because();
            ((Property<PropertyEntity, string>)sut.AllProperties.First()).ValueSetter.Invoke(entity, null, "expected");
        }

        [Test]
        public void should_invoke_the_custom_setter()
        {
            propertySetter.AssertWasCalled(x => x.Invoke(entity, "expected"));
        }
    }

    [TestFixture]
    public class When_a_checked_reference_is_added : With_persistence_specification<ReferenceEntity>
    {
        public override void because()
        {
            sut.CheckReference(x => x.Reference, new OtherEntity());
        }

        [Test]
        public void should_add_a_reference_property_check()
        {
            sut.AllProperties.First().ShouldBeOfType(typeof(ReferenceProperty<ReferenceEntity, object>));
        }

        [Test]
        public void should_add_one_check_to_the_specification()
        {
            sut.AllProperties.ShouldHaveCount(1);
        }

        [Test]
        public void should_set_the_custom_equality_comparer()
        {
            sut.AllProperties.First().EntityEqualityComparer.ShouldEqual(comparer);
        }
    }

    [TestFixture]
    public class When_a_checked_reference_with_a_custom_setter_is_added : With_persistence_specification<ReferenceEntity>
    {
        protected Action<ReferenceEntity, OtherEntity> propertySetter;

        public override void establish_context()
        {
            base.establish_context();
            propertySetter = MockRepository.GenerateStub<Action<ReferenceEntity, OtherEntity>>();
        }

        public override void because()
        {
            sut.CheckReference(x => x.Reference, new OtherEntity(), propertySetter);
        }

        [Test]
        public void should_add_a_reference_property_check()
        {
            sut.AllProperties.First().ShouldBeOfType(typeof(ReferenceProperty<ReferenceEntity, OtherEntity>));
        }

        [Test]
        public void should_add_one_check_to_the_specification()
        {
            sut.AllProperties.ShouldHaveCount(1);
        }

        [Test]
        public void should_set_the_custom_equality_comparer()
        {
            sut.AllProperties.First().EntityEqualityComparer.ShouldEqual(comparer);
        }
    }

    [TestFixture]
    public class When_the_value_setter_of_a_checked_reference_is_invoked : When_a_checked_reference_with_a_custom_setter_is_added
    {
        private ReferenceEntity entity;
        private OtherEntity referenced;

        public override void establish_context()
        {
            base.establish_context();
            entity = new ReferenceEntity();
            referenced = new OtherEntity();
        }

        public override void because()
        {
            base.because();
            ((Property<ReferenceEntity, OtherEntity>)sut.AllProperties.First()).ValueSetter.Invoke(entity, null, referenced);
        }

        [Test]
        public void should_invoke_the_custom_setter()
        {
            propertySetter.AssertWasCalled(x => x.Invoke(entity, referenced));
        }
    }

    [TestFixture]
    public class When_a_checked_list_is_added : With_persistence_specification<ReferenceEntity>
    {
        public override void because()
        {
            sut.CheckList(x => x.ReferenceList, new[] {new OtherEntity(), new OtherEntity()});
        }

        [Test]
        public void should_add_a_reference_list_check()
        {
            sut.AllProperties.First().ShouldBeOfType(typeof(ReferenceList<ReferenceEntity, OtherEntity>));
        }

        [Test]
        public void should_add_one_check_to_the_specification()
        {
            sut.AllProperties.ShouldHaveCount(1);
        }

        [Test]
        public void should_set_the_custom_equality_comparer()
        {
            sut.AllProperties.First().EntityEqualityComparer.ShouldEqual(comparer);
        }
    }

    [TestFixture]
    public class When_a_checked_enumerable_with_a_custom_item_setter_is_added : With_persistence_specification<ReferenceEntity>
    {
        protected Action<ReferenceEntity, OtherEntity> listSetter;

        public override void establish_context()
        {
            base.establish_context();
            listSetter = MockRepository.GenerateStub<Action<ReferenceEntity, OtherEntity>>();
        }

        public override void because()
        {
#pragma warning disable 618,612
            sut.CheckEnumerable(x => x.ReferenceList, listSetter, new[] {new OtherEntity(), new OtherEntity()});
#pragma warning restore 618,612
        }

        [Test]
        public void should_add_a_reference_list_check()
        {
            sut.AllProperties.First().ShouldBeOfType(typeof(ReferenceList<ReferenceEntity, OtherEntity>));
        }

        [Test]
        public void should_add_one_check_to_the_specification()
        {
            sut.AllProperties.ShouldHaveCount(1);
        }

        [Test]
        public void should_set_the_custom_equality_comparer()
        {
            sut.AllProperties.First().EntityEqualityComparer.ShouldEqual(comparer);
        }
    }

    [TestFixture]
    public class When_a_checked_list_with_a_custom_list_setter_is_added : With_persistence_specification<ReferenceEntity>
    {
        protected Action<ReferenceEntity, IEnumerable<OtherEntity>> listSetter;

        public override void establish_context()
        {
            base.establish_context();
            listSetter = MockRepository.GenerateStub<Action<ReferenceEntity, IEnumerable<OtherEntity>>>();
        }

        public override void because()
        {
            sut.CheckList(x => x.ReferenceList, new[] {new OtherEntity(), new OtherEntity()}, listSetter);
        }

        [Test]
        public void should_add_a_reference_list_check()
        {
            sut.AllProperties.First().ShouldBeOfType(typeof(ReferenceList<ReferenceEntity, OtherEntity>));
        }

        [Test]
        public void should_add_one_check_to_the_specification()
        {
            sut.AllProperties.ShouldHaveCount(1);
        }

        [Test]
        public void should_set_the_custom_equality_comparer()
        {
            sut.AllProperties.First().EntityEqualityComparer.ShouldEqual(comparer);
        }
    }

    [TestFixture]
    public class When_the_list_setter_of_a_checked_list_is_invoked : When_a_checked_list_with_a_custom_list_setter_is_added
    {
        private ReferenceEntity entity;
        private OtherEntity[] referenced;

        public override void establish_context()
        {
            base.establish_context();
            entity = new ReferenceEntity();
            referenced = new[] {new OtherEntity(), new OtherEntity()};
        }

        public override void because()
        {
            base.because();
            ((ReferenceList<ReferenceEntity, OtherEntity>)sut.AllProperties.First()).ValueSetter.Invoke(entity, null, referenced);
        }

        [Test]
        public void should_invoke_the_custom_setter()
        {
            listSetter.AssertWasCalled(x => x.Invoke(entity, referenced));
        }
    }

    [TestFixture]
    public class When_a_checked_list_with_a_custom_list_item_setter_is_added : With_persistence_specification<ReferenceEntity>
    {
        protected Action<ReferenceEntity, OtherEntity> listItemSetter;

        public override void establish_context()
        {
            base.establish_context();
            listItemSetter = MockRepository.GenerateStub<Action<ReferenceEntity, OtherEntity>>();
        }

        public override void because()
        {
            sut.CheckList(x => x.ReferenceList, new[] {new OtherEntity(), new OtherEntity()}, listItemSetter);
        }

        [Test]
        public void should_add_a_reference_list_check()
        {
            sut.AllProperties.First().ShouldBeOfType(typeof(ReferenceList<ReferenceEntity, OtherEntity>));
        }

        [Test]
        public void should_add_one_check_to_the_specification()
        {
            sut.AllProperties.ShouldHaveCount(1);
        }

        [Test]
        public void should_set_the_custom_equality_comparer()
        {
            sut.AllProperties.First().EntityEqualityComparer.ShouldEqual(comparer);
        }
    }

    [TestFixture]
    public class When_the_list_item_setter_of_a_checked_list_is_invoked : When_a_checked_list_with_a_custom_list_item_setter_is_added
    {
        private ReferenceEntity entity;
        private OtherEntity[] referenced;

        public override void establish_context()
        {
            base.establish_context();
            entity = new ReferenceEntity();
            referenced = new[] {new OtherEntity(), new OtherEntity()};
        }

        public override void because()
        {
            base.because();
            ((ReferenceList<ReferenceEntity, OtherEntity>)sut.AllProperties.First()).ValueSetter.Invoke(entity, null, referenced);
        }

        [Test]
        public void should_invoke_the_custom_setter_for_each_item()
        {
            listItemSetter.AssertWasCalled(x => x.Invoke(entity, referenced[0]));
            listItemSetter.AssertWasCalled(x => x.Invoke(entity, referenced[1]));
        }
    }

    [TestFixture]
    public class When_a_checked_component_list_is_added : With_persistence_specification<ReferenceEntity>
    {
        public override void because()
        {
            sut.CheckComponentList(x => x.ReferenceList, new[] {new OtherEntity(), new OtherEntity()});
        }

        [Test]
        public void should_add_a_list_check()
        {
            sut.AllProperties.First().ShouldBeOfType(typeof(List<ReferenceEntity, OtherEntity>));
        }

        [Test]
        public void should_add_one_check_to_the_specification()
        {
            sut.AllProperties.ShouldHaveCount(1);
        }

        [Test]
        public void should_set_the_custom_equality_comparer()
        {
            sut.AllProperties.First().EntityEqualityComparer.ShouldEqual(comparer);
        }
    }

    [TestFixture]
    public class When_a_checked_component_list_with_a_custom_list_setter_is_added : With_persistence_specification<ReferenceEntity>
    {
        protected Action<ReferenceEntity, IEnumerable<OtherEntity>> listSetter;

        public override void establish_context()
        {
            base.establish_context();
            listSetter = MockRepository.GenerateStub<Action<ReferenceEntity, IEnumerable<OtherEntity>>>();
        }

        public override void because()
        {
            sut.CheckComponentList(x => x.ReferenceList, new[] {new OtherEntity(), new OtherEntity()}, listSetter);
        }

        [Test]
        public void should_add_a_reference_property_check()
        {
            sut.AllProperties.First().ShouldBeOfType(typeof(List<ReferenceEntity, OtherEntity>));
        }

        [Test]
        public void should_add_one_check_to_the_specification()
        {
            sut.AllProperties.ShouldHaveCount(1);
        }

        [Test]
        public void should_set_the_custom_equality_comparer()
        {
            sut.AllProperties.First().EntityEqualityComparer.ShouldEqual(comparer);
        }
    }

    [TestFixture]
    public class When_the_list_setter_of_a_checked_component_list_is_invoked : When_a_checked_component_list_with_a_custom_list_setter_is_added
    {
        private ReferenceEntity entity;
        private OtherEntity[] referenced;

        public override void establish_context()
        {
            base.establish_context();
            entity = new ReferenceEntity();
            referenced = new[] {new OtherEntity(), new OtherEntity()};
        }

        public override void because()
        {
            base.because();
            ((List<ReferenceEntity, OtherEntity>)sut.AllProperties.First()).ValueSetter.Invoke(entity, null, referenced);
        }

        [Test]
        public void should_invoke_the_custom_setter()
        {
            listSetter.AssertWasCalled(x => x.Invoke(entity, referenced));
        }
    }

    [TestFixture]
    public class When_a_checked_component_list_with_a_custom_list_item_setter_is_added : With_persistence_specification<ReferenceEntity>
    {
        protected Action<ReferenceEntity, OtherEntity> listItemSetter;

        public override void establish_context()
        {
            base.establish_context();
            listItemSetter = MockRepository.GenerateStub<Action<ReferenceEntity, OtherEntity>>();
        }

        public override void because()
        {
            sut.CheckComponentList(x => x.ReferenceList, new[] {new OtherEntity(), new OtherEntity()}, listItemSetter);
        }

        [Test]
        public void should_add_a_reference_property_check()
        {
            sut.AllProperties.First().ShouldBeOfType(typeof(List<ReferenceEntity, OtherEntity>));
        }

        [Test]
        public void should_add_one_check_to_the_specification()
        {
            sut.AllProperties.ShouldHaveCount(1);
        }

        [Test]
        public void should_set_the_custom_equality_comparer()
        {
            sut.AllProperties.First().EntityEqualityComparer.ShouldEqual(comparer);
        }
    }

    [TestFixture]
    public class When_the_list_item_setter_of_a_checked_component_list_is_invoked : When_a_checked_component_list_with_a_custom_list_item_setter_is_added
    {
        private ReferenceEntity entity;
        private OtherEntity[] referenced;

        public override void establish_context()
        {
            base.establish_context();
            entity = new ReferenceEntity();
            referenced = new[] {new OtherEntity(), new OtherEntity()};
        }

        public override void because()
        {
            base.because();
            ((List<ReferenceEntity, OtherEntity>)sut.AllProperties.First()).ValueSetter.Invoke(entity, null, referenced);
        }

        [Test]
        public void should_invoke_the_custom_setter_for_each_item()
        {
            listItemSetter.AssertWasCalled(x => x.Invoke(entity, referenced[0]));
            listItemSetter.AssertWasCalled(x => x.Invoke(entity, referenced[1]));
        }
    }
}
