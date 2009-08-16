using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection
{
    [TestFixture, Category("Inspection DSL")]
    public class CompositeIdInspectorMapsToCompositeIdMapping
    {
        private CompositeIdMapping mapping;
        private ICompositeIdentityInspector inspector;

        [SetUp]
        public void CreateDsl()
        {
            mapping = new CompositeIdMapping();
            inspector = new CompositeIdentityInspector(mapping);
        }

        [Test]
        public void AccessMapped()
        {
            mapping.Access = "field";
            inspector.Access.ShouldEqual(Access.Field);
        }

        [Test]
        public void AccessIsSet()
        {
            mapping.Access = "field";
            inspector.IsSet(Prop(x => x.Access))
                .ShouldBeTrue();
        }

        [Test]
        public void AccessIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Access))
                .ShouldBeFalse();
        }

        [Test]
        public void ClassMapped()
        {
            mapping.Class = new TypeReference(typeof(ExampleClass));
            inspector.Class.ShouldEqual(new TypeReference(typeof(ExampleClass)));
        }

        [Test]
        public void ClassIsSet()
        {
            mapping.Class = new TypeReference(typeof(ExampleClass));
            inspector.IsSet(Prop(x => x.Class))
                .ShouldBeTrue();
        }

        [Test]
        public void ClassIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Class))
                .ShouldBeFalse();
        }

        [Test]
        public void KeyManyToOnesCollectionHasSameCountAsMapping()
        {
            mapping.AddKeyManyToOne(new KeyManyToOneMapping());
            inspector.KeyManyToOnes.Count().ShouldEqual(1);
        }

        [Test]
        public void KeyManyToOnesCollectionOfInspectors()
        {
            mapping.AddKeyManyToOne(new KeyManyToOneMapping());
            inspector.KeyManyToOnes.First().ShouldBeOfType<IKeyManyToOneInspector>();
        }

        [Test]
        public void KeyManyToOnesCollectionIsEmpty()
        {
            inspector.KeyManyToOnes.IsEmpty().ShouldBeTrue();
        }

        [Test]
        public void KeyPropertiesCollectionHasSameCountAsMapping()
        {
            mapping.AddKeyProperty(new KeyPropertyMapping());
            inspector.KeyProperties.Count().ShouldEqual(1);
        }

        [Test]
        public void KeyPropertiesCollectionOfInspectors()
        {
            mapping.AddKeyProperty(new KeyPropertyMapping());
            inspector.KeyProperties.First().ShouldBeOfType<IKeyPropertyInspector>();
        }

        [Test]
        public void KeyPropertiesCollectionIsEmpty()
        {
            inspector.KeyProperties.IsEmpty().ShouldBeTrue();
        }

        [Test]
        public void NameMapped()
        {
            mapping.Name = "name";
            inspector.Name.ShouldEqual("name");
        }

        [Test]
        public void NameIsSet()
        {
            mapping.Name = "name";
            inspector.IsSet(Prop(x => x.Name))
                .ShouldBeTrue();
        }

        [Test]
        public void NameIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Name))
                .ShouldBeFalse();
        }

        [Test]
        public void MappedMapped()
        {
            mapping.Mapped = true;
            inspector.Mapped.ShouldEqual(true);
        }

        [Test]
        public void MappedIsSet()
        {
            mapping.Mapped = true;
            inspector.IsSet(Prop(x => x.Mapped))
                .ShouldBeTrue();
        }

        [Test]
        public void MappedIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Mapped))
                .ShouldBeFalse();
        }

        [Test]
        public void UnsavedValueMapped()
        {
            mapping.UnsavedValue = "value";
            inspector.UnsavedValue.ShouldEqual("value");
        }

        [Test]
        public void UnsavedValueIsSet()
        {
            mapping.UnsavedValue = "value";
            inspector.IsSet(Prop(x => x.UnsavedValue))
                .ShouldBeTrue();
        }

        [Test]
        public void UnsavedValueIsNotSet()
        {
            inspector.IsSet(Prop(x => x.UnsavedValue))
                .ShouldBeFalse();
        }

        #region Helpers

        private PropertyInfo Prop(Expression<Func<ICompositeIdentityInspector, object>> propertyExpression)
        {
            return ReflectionHelper.GetProperty(propertyExpression);
        }

        #endregion
    }
}