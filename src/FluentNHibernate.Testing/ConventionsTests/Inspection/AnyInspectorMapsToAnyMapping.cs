using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection
{
    [TestFixture, Category("Inspection DSL")]
    public class AnyInspectorMapsToAnyMapping
    {
        private AnyMapping mapping;
        private IAnyInspector inspector;

        [SetUp]
        public void CreateDsl()
        {
            mapping = new AnyMapping();
            inspector = new AnyInspector(mapping);
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
        public void CascadeMapped()
        {
            mapping.Cascade = "all";
            inspector.Cascade.ShouldEqual(Cascade.All);
        }

        [Test]
        public void CascadeIsSet()
        {
            mapping.Cascade = "all";
            inspector.IsSet(Prop(x => x.Cascade))
                .ShouldBeTrue();
        }

        [Test]
        public void CascadeIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Cascade))
                .ShouldBeFalse();
        }

        [Test]
        public void IdentifierColumnsCollectionHasSameCountAsMapping()
        {
            mapping.AddIdentifierColumn(new ColumnMapping());
            inspector.IdentifierColumns.Count().ShouldEqual(1);
        }

        [Test]
        public void IdentifierColumnsCollectionOfInspectors()
        {
            mapping.AddIdentifierColumn(new ColumnMapping());
            inspector.IdentifierColumns.First().ShouldBeOfType<IColumnInspector>();
        }

        [Test]
        public void IdentifierColumnsCollectionIsEmpty()
        {
            inspector.IdentifierColumns.IsEmpty().ShouldBeTrue();
        }

        [Test]
        public void IdTypeMapped()
        {
            mapping.IdType = "type";
            inspector.IdType.ShouldEqual("type");
        }

        [Test]
        public void IdTypeIsSet()
        {
            mapping.IdType = "type";
            inspector.IsSet(Prop(x => x.IdType))
                .ShouldBeTrue();
        }

        [Test]
        public void IdTypeIsNotSet()
        {
            inspector.IsSet(Prop(x => x.IdType))
                .ShouldBeFalse();
        }

        [Test]
        public void InsertMapped()
        {
            mapping.Insert = true;
            inspector.Insert.ShouldEqual(true);
        }

        [Test]
        public void InsertIsSet()
        {
            mapping.Insert = true;
            inspector.IsSet(Prop(x => x.Insert))
                .ShouldBeTrue();
        }

        [Test]
        public void InsertIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Insert))
                .ShouldBeFalse();
        }

        [Test]
        public void LazyMapped()
        {
            mapping.Lazy = true;
            inspector.LazyLoad.ShouldEqual(true);
        }

        [Test]
        public void LazyIsSet()
        {
            mapping.Lazy = true;
            inspector.IsSet(Prop(x => x.LazyLoad))
                .ShouldBeTrue();
        }

        [Test]
        public void LazyIsNotSet()
        {
            inspector.IsSet(Prop(x => x.LazyLoad))
                .ShouldBeFalse();
        }

        [Test]
        public void MetaTypeMapped()
        {
            mapping.MetaType = new TypeReference(typeof(string));
            inspector.MetaType.ShouldEqual(new TypeReference(typeof(string)));
        }

        [Test]
        public void MetaTypeIsSet()
        {
            mapping.MetaType = new TypeReference(typeof(string));
            inspector.IsSet(Prop(x => x.MetaType))
                .ShouldBeTrue();
        }

        [Test]
        public void MetaTypeIsNotSet()
        {
            inspector.IsSet(Prop(x => x.MetaType))
                .ShouldBeFalse();
        }

        [Test]
        public void MetaValuesCollectionHasSameCountAsMapping()
        {
            mapping.AddMetaValue(new MetaValueMapping());
            inspector.MetaValues.Count().ShouldEqual(1);
        }

        [Test]
        public void MetaValuesCollectionOfInspectors()
        {
            mapping.AddMetaValue(new MetaValueMapping());
            inspector.MetaValues.First().ShouldBeOfType<IMetaValueInspector>();
        }

        [Test]
        public void MetaValuesCollectionIsEmpty()
        {
            inspector.MetaValues.IsEmpty().ShouldBeTrue();
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
        public void OptimisticLockMapped()
        {
            mapping.OptimisticLock = true;
            inspector.OptimisticLock.ShouldEqual(true);
        }

        [Test]
        public void OptimisticLockIsSet()
        {
            mapping.OptimisticLock = true;
            inspector.IsSet(Prop(x => x.OptimisticLock))
                .ShouldBeTrue();
        }

        [Test]
        public void OptimisticLockIsNotSet()
        {
            inspector.IsSet(Prop(x => x.OptimisticLock))
                .ShouldBeFalse();
        }

        [Test]
        public void TypeColumnsCollectionHasSameCountAsMapping()
        {
            mapping.AddTypeColumn(new ColumnMapping());
            inspector.TypeColumns.Count().ShouldEqual(1);
        }

        [Test]
        public void TypeColumnsCollectionOfInspectors()
        {
            mapping.AddTypeColumn(new ColumnMapping());
            inspector.TypeColumns.First().ShouldBeOfType<IColumnInspector>();
        }

        [Test]
        public void TypeColumnsCollectionIsEmpty()
        {
            inspector.TypeColumns.IsEmpty().ShouldBeTrue();
        }

        [Test]
        public void UpdateMapped()
        {
            mapping.Update = true;
            inspector.Update.ShouldEqual(true);
        }

        [Test]
        public void UpdateIsSet()
        {
            mapping.Update = true;
            inspector.IsSet(Prop(x => x.Update))
                .ShouldBeTrue();
        }

        [Test]
        public void UpdateIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Update))
                .ShouldBeFalse();
        }

        #region Helpers

        private PropertyInfo Prop(Expression<Func<IAnyInspector, object>> propertyExpression)
        {
            return ReflectionHelper.GetProperty(propertyExpression);
        }

        #endregion
    }
}