using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection
{
    [TestFixture, Category("Inspection DSL")]
    public class JoinedSubclassInspectorMapsToJoinedSubclassMapping
    {
        private JoinedSubclassMapping mapping;
        private IJoinedSubclassInspector inspector;

        [SetUp]
        public void CreateDsl()
        {
            mapping = new JoinedSubclassMapping();
            inspector = new JoinedSubclassInspector(mapping);
        }

        [Test]
        public void AbstractMapped()
        {
            mapping.Abstract = true;
            inspector.Abstract.ShouldEqual(true);
        }

        [Test]
        public void AbstractIsSet()
        {
            mapping.Abstract = true;
            inspector.IsSet(Prop(x => x.Abstract))
                .ShouldBeTrue();
        }

        [Test]
        public void AbstractIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Abstract))
                .ShouldBeFalse();
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
        public void CheckMapped()
        {
            mapping.Check = "x";
            inspector.Check.ShouldEqual("x");
        }

        [Test]
        public void CheckIsSet()
        {
            mapping.Check = "x";
            inspector.IsSet(Prop(x => x.Check))
                .ShouldBeTrue();
        }

        [Test]
        public void CheckIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Check))
                .ShouldBeFalse();
        }

        [Test]
        public void CollectionsCollectionHasSameCountAsMapping()
        {
            mapping.AddCollection(new BagMapping());
            inspector.Collections.Count().ShouldEqual(1);
        }

        [Test]
        public void CollectionsCollectionOfInspectors()
        {
            mapping.AddCollection(new BagMapping());
            inspector.Collections.First().ShouldBeOfType<ICollectionInspector>();
        }

        [Test]
        public void CollectionsCollectionIsEmpty()
        {
            inspector.Collections.IsEmpty().ShouldBeTrue();
        }

        [Test]
        public void DynamicInsertMapped()
        {
            mapping.DynamicInsert = true;
            inspector.DynamicInsert.ShouldEqual(true);
        }

        [Test]
        public void DynamicInsertIsSet()
        {
            mapping.DynamicInsert = true;
            inspector.IsSet(Prop(x => x.DynamicInsert))
                .ShouldBeTrue();
        }

        [Test]
        public void DynamicInsertIsNotSet()
        {
            inspector.IsSet(Prop(x => x.DynamicInsert))
                .ShouldBeFalse();
        }

        [Test]
        public void DynamicUpdateMapped()
        {
            mapping.DynamicUpdate = true;
            inspector.DynamicUpdate.ShouldEqual(true);
        }

        [Test]
        public void DynamicUpdateIsSet()
        {
            mapping.DynamicUpdate = true;
            inspector.IsSet(Prop(x => x.DynamicUpdate))
                .ShouldBeTrue();
        }

        [Test]
        public void DynamicUpdateIsNotSet()
        {
            inspector.IsSet(Prop(x => x.DynamicUpdate))
                .ShouldBeFalse();
        }

        [Test]
        public void ExtendsMapped()
        {
            mapping.Extends = "other-class";
            inspector.Extends.ShouldEqual("other-class");
        }

        [Test]
        public void ExtendsIsSet()
        {
            mapping.Extends = "other-class";
            inspector.IsSet(Prop(x => x.Extends))
                .ShouldBeTrue();
        }

        [Test]
        public void ExtendsIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Extends))
                .ShouldBeFalse();
        }

        [Test]
        public void JoinsCollectionHasSameCountAsMapping()
        {
            mapping.AddJoin(new JoinMapping());
            inspector.Joins.Count().ShouldEqual(1);
        }

        [Test]
        public void JoinsCollectionOfInspectors()
        {
            mapping.AddJoin(new JoinMapping());
            inspector.Joins.First().ShouldBeOfType<IJoinInspector>();
        }

        [Test]
        public void JoinsCollectionIsEmpty()
        {
            inspector.Joins.IsEmpty().ShouldBeTrue();
        }

        [Test]
        public void KeyMapped()
        {
            mapping.Key = new KeyMapping();
            mapping.Key.ForeignKey = "test";
            inspector.Key.ForeignKey.ShouldEqual("test");
        }

        [Test]
        public void KeyIsSet()
        {
            mapping.Key = new KeyMapping();
            mapping.Key.ForeignKey = "test";
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
        public void OneToOnesCollectionHasSameCountAsMapping()
        {
            mapping.AddOneToOne(new OneToOneMapping());
            inspector.OneToOnes.Count().ShouldEqual(1);
        }

        [Test]
        public void OneToOnesCollectionOfInspectors()
        {
            mapping.AddOneToOne(new OneToOneMapping());
            inspector.OneToOnes.First().ShouldBeOfType<IOneToOneInspector>();
        }

        [Test]
        public void OneToOnesCollectionIsEmpty()
        {
            inspector.OneToOnes.IsEmpty().ShouldBeTrue();
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
            inspector.Properties.First().ShouldBeOfType<IPropertyInspector>();
        }

        [Test]
        public void PropertiesCollectionIsEmpty()
        {
            inspector.Properties.IsEmpty().ShouldBeTrue();
        }

        [Test]
        public void ProxyMapped()
        {
            mapping.Proxy = "proxy";
            inspector.Proxy.ShouldEqual("proxy");
        }

        [Test]
        public void ProxyIsSet()
        {
            mapping.Proxy = "proxy";
            inspector.IsSet(Prop(x => x.Proxy))
                .ShouldBeTrue();
        }

        [Test]
        public void ProxyIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Proxy))
                .ShouldBeFalse();
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
            inspector.References.First().ShouldBeOfType<IManyToOneInspector>();
        }

        [Test]
        public void ReferencesCollectionIsEmpty()
        {
            inspector.References.IsEmpty().ShouldBeTrue();
        }

        [Test]
        public void SelectBeforeUpdateMapped()
        {
            mapping.SelectBeforeUpdate = true;
            inspector.SelectBeforeUpdate.ShouldEqual(true);
        }

        [Test]
        public void SelectBeforeUpdateIsSet()
        {
            mapping.SelectBeforeUpdate = true;
            inspector.IsSet(Prop(x => x.SelectBeforeUpdate))
                .ShouldBeTrue();
        }

        [Test]
        public void SelectBeforeUpdateIsNotSet()
        {
            inspector.IsSet(Prop(x => x.SelectBeforeUpdate))
                .ShouldBeFalse();
        }

        [Test]
        public void SubclassesCollectionHasSameCountAsMapping()
        {
            mapping.AddSubclass(new JoinedSubclassMapping());
            inspector.Subclasses.Count().ShouldEqual(1);
        }

        [Test]
        public void SubclassesCollectionOfInspectors()
        {
            mapping.AddSubclass(new JoinedSubclassMapping());
            inspector.Subclasses.First().ShouldBeOfType<IJoinedSubclassInspector>();
        }

        [Test]
        public void SubclassesCollectionIsEmpty()
        {
            inspector.Subclasses.IsEmpty().ShouldBeTrue();
        }

        [Test]
        public void TableNameMapped()
        {
            mapping.TableName = "table";
            inspector.TableName.ShouldEqual("table");
        }

        [Test]
        public void TableNameIsSet()
        {
            mapping.TableName = "table";
            inspector.IsSet(Prop(x => x.TableName))
                .ShouldBeTrue();
        }

        [Test]
        public void TableNameIsNotSet()
        {
            inspector.IsSet(Prop(x => x.TableName))
                .ShouldBeFalse();
        }

        [Test]
        public void TypeMapped()
        {
            mapping.Type = typeof(ExampleClass);
            inspector.Type.ShouldEqual(typeof(ExampleClass));
        }

        [Test]
        public void TypeIsSet()
        {
            mapping.Type = typeof(ExampleClass);
            inspector.IsSet(Prop(x => x.Type))
                .ShouldBeTrue();
        }

        [Test]
        public void TypeIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Type))
                .ShouldBeFalse();
        }

        #region Helpers

        private PropertyInfo Prop(Expression<Func<IJoinedSubclassInspector, object>> propertyExpression)
        {
            return ReflectionHelper.GetProperty(propertyExpression);
        }

        #endregion
    }
}