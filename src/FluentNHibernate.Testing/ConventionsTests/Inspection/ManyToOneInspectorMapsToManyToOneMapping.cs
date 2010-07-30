using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils.Reflection;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection
{
    [TestFixture, Category("Inspection DSL")]
    public class ManyToOneInspectorMapsToManyToOneMapping
    {
        private ManyToOneMapping mapping;
        private IManyToOneInspector inspector;

        [SetUp]
        public void CreateDsl()
        {
            mapping = new ManyToOneMapping();
            inspector = new ManyToOneInspector(mapping);
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
        public void ClassMapped()
        {
            mapping.Class = new TypeReference(typeof(string));
            inspector.Class.ShouldEqual(new TypeReference(typeof(string)));
        }

        [Test]
        public void ClassIsSet()
        {
            mapping.Class = new TypeReference(typeof(string));
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
        public void ColumnsCollectionHasSameCountAsMapping()
        {
            mapping.AddColumn(new ColumnMapping());
            inspector.Columns.Count().ShouldEqual(1);
        }

        [Test]
        public void ColumnsCollectionOfInspectors()
        {
            mapping.AddColumn(new ColumnMapping());
            inspector.Columns.First().ShouldBeOfType<IColumnInspector>();
        }

        [Test]
        public void ColumnsCollectionIsEmpty()
        {
            inspector.Columns.IsEmpty().ShouldBeTrue();
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
        public void ForeignKeyMapped()
        {
            mapping.ForeignKey = "key";
            inspector.ForeignKey.ShouldEqual("key");
        }

        [Test]
        public void ForeignKeyIsSet()
        {
            mapping.ForeignKey = "key";
            inspector.IsSet(Prop(x => x.ForeignKey))
                .ShouldBeTrue();
        }

        [Test]
        public void ForeignKeyIsNotSet()
        {
            inspector.IsSet(Prop(x => x.ForeignKey))
                .ShouldBeFalse();
        }

        [Test]
        public void InsertMapped()
        {
            mapping.Insert = true;
            inspector.Insert.ShouldBeTrue();
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
        public void LazyLoadMapped()
        {
            mapping.Lazy = Laziness.Proxy.ToString();
            inspector.LazyLoad.ShouldEqual(Laziness.Proxy);
        }

        [Test]
        public void LazyLoadIsSet()
        {
            mapping.Lazy = Laziness.Proxy.ToString();
            inspector.IsSet(Prop(x => x.LazyLoad))
                .ShouldBeTrue();
        }

        [Test]
        public void LazyLoadIsNotSet()
        {
            inspector.IsSet(Prop(x => x.LazyLoad))
                .ShouldBeFalse();
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
        public void NotFoundMapped()
        {
            mapping.NotFound = "exception";
            inspector.NotFound.ShouldEqual(NotFound.Exception);
        }

        [Test]
        public void NotFoundIsSet()
        {
            mapping.NotFound = "exception";
            inspector.IsSet(Prop(x => x.NotFound))
                .ShouldBeTrue();
        }

        [Test]
        public void NotFoundIsNotSet()
        {
            inspector.IsSet(Prop(x => x.NotFound))
                .ShouldBeFalse();
        }

        [Test]
        public void PropertyRefMapped()
        {
            mapping.PropertyRef = "ref";
            inspector.PropertyRef.ShouldEqual("ref");
        }

        [Test]
        public void PropertyRefIsSet()
        {
            mapping.PropertyRef = "ref";
            inspector.IsSet(Prop(x => x.PropertyRef))
                .ShouldBeTrue();
        }

        [Test]
        public void PropertyRefIsNotSet()
        {
            inspector.IsSet(Prop(x => x.PropertyRef))
                .ShouldBeFalse();
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

        [Test]
        public void NullableMapped()
        {
            mapping.AddColumn(new ColumnMapping { NotNull = false });
            inspector.Nullable.ShouldEqual(true);
        }

        [Test]
        public void NullableIsSet()
        {
            mapping.AddColumn(new ColumnMapping { NotNull = false });
            inspector.IsSet(Prop(x => x.Nullable))
                .ShouldBeTrue();
        }

        [Test]
        public void NullableIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Nullable))
                .ShouldBeFalse();
        }
        
        [Test]
        public void FormulaMapped()
        {
            mapping.Formula = "formula";
            inspector.Formula.ShouldEqual(mapping.Formula);
        }
        
        [Test]
        public void FormulaIsSet()
        {
            mapping.Formula = "formula";
            inspector.IsSet(Prop(x => x.Formula))
                .ShouldBeTrue();
        }
        
        [Test]
        public void FormulaIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Formula))
                .ShouldBeFalse();
        }

        #region Helpers

        private Member Prop(Expression<Func<IManyToOneInspector, object>> propertyExpression)
        {
            return ReflectionHelper.GetMember(propertyExpression);
        }

        #endregion
    }
}