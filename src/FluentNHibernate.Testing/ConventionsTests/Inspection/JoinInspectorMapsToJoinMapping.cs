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
    public class JoinInspectorMapsToJoinMapping
    {
        private JoinMapping mapping;
        private IJoinInspector inspector;

        [SetUp]
        public void CreateDsl()
        {
            mapping = new JoinMapping();
            inspector = new JoinInspector(mapping);
        }

        [Test]
        public void AnysCollectionHasSameCountAsMapping()
        {
            mapping.AddAny(new AnyMapping());
            inspector.Anys.Count().ShouldEqual(1);
        }

        [Test]
        public void AnysCollectionOfInspectors()
        {
            mapping.AddAny(new AnyMapping());
            inspector.Anys.First().ShouldBeOfType<IAnyInspector>();
        }

        [Test]
        public void AnysCollectionIsEmpty()
        {
            inspector.Anys.IsEmpty().ShouldBeTrue();
        }

        [Test]
        public void CatalogMapped()
        {
            mapping.Catalog = "value";
            inspector.Catalog.ShouldEqual("value");
        }

        [Test]
        public void CatalogIsSet()
        {
            mapping.Catalog = "value";
            inspector.IsSet(Prop(x => x.Catalog))
                .ShouldBeTrue();
        }

        [Test]
        public void CatalogIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Catalog))
                .ShouldBeFalse();
        }

        [Test]
        public void FetchMapped()
        {
            mapping.Fetch = "join";
            inspector.Fetch.ShouldEqual(Fetch.Join);
        }

        [Test]
        public void FetchIsSet()
        {
            mapping.Fetch = "join";
            inspector.IsSet(Prop(x => x.Fetch))
                .ShouldBeTrue();
        }

        [Test]
        public void FetchIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Fetch))
                .ShouldBeFalse();
        }

        [Test]
        public void InverseMapped()
        {
            mapping.Inverse = true;
            inspector.Inverse.ShouldEqual(true);
        }

        [Test]
        public void InverseIsSet()
        {
            mapping.Inverse = true;
            inspector.IsSet(Prop(x => x.Inverse))
                .ShouldBeTrue();
        }

        [Test]
        public void InverseIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Inverse))
                .ShouldBeFalse();
        }

        [Test]
        public void KeyMapped()
        {
            mapping.Key = new KeyMapping();
            mapping.Key.PropertyRef = "ref";
            inspector.Key.PropertyRef.ShouldEqual("ref");
        }

        [Test]
        public void KeyIsSet()
        {
            mapping.Key = new KeyMapping();
            mapping.Key.PropertyRef = "ref";
            inspector.IsSet(Prop(x => x.Key))
                .ShouldBeTrue();
        }

        [Test]
        public void KeyIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Key))
                .ShouldBeFalse();
        }

        [Test]
        public void OptionalMapped()
        {
            mapping.Optional = true;
            inspector.Optional.ShouldEqual(true);
        }

        [Test]
        public void OptionalIsSet()
        {
            mapping.Optional = true;
            inspector.IsSet(Prop(x => x.Optional))
                .ShouldBeTrue();
        }

        [Test]
        public void OptionalIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Optional))
                .ShouldBeFalse();
        }

        [Test]
        public void PropertiesCollectionHasSameCountAsMapping()
        {
            mapping.AddProperty(new PropertyMapping());
            inspector.Properties.Count().ShouldEqual(1);
        }

        [Test]
        public void PropertiesCollectionOfInspectors()
        {
            mapping.AddProperty(new PropertyMapping());
            inspector.Properties.First().ShouldImplementType<IPropertyInspector>();
        }

        [Test]
        public void PropertiesCollectionIsEmpty()
        {
            inspector.Properties.IsEmpty().ShouldBeTrue();
        }

        [Test]
        public void ReferencesCollectionHasSameCountAsMapping()
        {
            mapping.AddReference(new ManyToOneMapping());
            inspector.References.Count().ShouldEqual(1);
        }

        [Test]
        public void ReferencesCollectionOfInspectors()
        {
            mapping.AddReference(new ManyToOneMapping());
            inspector.References.First().ShouldImplementType<IManyToOneInspector>();
        }

        [Test]
        public void ReferencesCollectionIsEmpty()
        {
            inspector.References.IsEmpty().ShouldBeTrue();
        }

        [Test]
        public void SchemaMapped()
        {
            mapping.Schema = "dbo";
            inspector.Schema.ShouldEqual("dbo");
        }

        [Test]
        public void SchemaIsSet()
        {
            mapping.Schema = "dbo";
            inspector.IsSet(Prop(x => x.Schema))
                .ShouldBeTrue();
        }

        [Test]
        public void SchemaIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Schema))
                .ShouldBeFalse();
        }

        [Test]
        public void SubselectMapped()
        {
            mapping.Subselect = "value";
            inspector.Subselect.ShouldEqual("value");
        }

        [Test]
        public void SubselectIsSet()
        {
            mapping.Subselect = "value";
            inspector.IsSet(Prop(x => x.Subselect))
                .ShouldBeTrue();
        }

        [Test]
        public void SubselectIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Subselect))
                .ShouldBeFalse();
        }

        [Test]
        public void TableNameMapped()
        {
            mapping.TableName = "tbl";
            inspector.TableName.ShouldEqual("tbl");
        }

        [Test]
        public void TableNameIsSet()
        {
            mapping.TableName = "tbl";
            inspector.IsSet(Prop(x => x.TableName))
                .ShouldBeTrue();
        }

        [Test]
        public void TableNameIsNotSet()
        {
            inspector.IsSet(Prop(x => x.TableName))
                .ShouldBeFalse();
        }

        #region Helpers

        private PropertyInfo Prop(Expression<Func<IJoinInspector, object>> propertyExpression)
        {
            return ReflectionHelper.GetProperty(propertyExpression);
        }

        #endregion
    }
}