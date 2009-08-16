using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class AnyMutablePropertyModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void AccessSetsModelAccessPropertyToValue()
        {
            Any<SecondMappedObject>()
                .Mapping(m => m
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col")
                    .EntityTypeColumn("col2")
                    .Access.Field())
                .ModelShouldMatch(x => x.Access.ShouldEqual("field"));
        }

        [Test]
        public void CascadeSetsModelCascadePropertyToValue()
        {
            Any<SecondMappedObject>()
                .Mapping(m => m
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col")
                    .EntityTypeColumn("col2")
                    .Cascade.All())
                .ModelShouldMatch(x => x.Cascade.ShouldEqual("all"));
        }

        [Test]
        public void IdentityTypeSetsModelIdTypePropertyToPropertyTypeName()
        {
            Any<SecondMappedObject>()
                .Mapping(m => m
                    .IdentityType(x => x.Id)
                    .EntityIdentifierColumn("col")
                    .EntityTypeColumn("col2"))
                .ModelShouldMatch(x => x.IdType.ShouldEqual(typeof(long).AssemblyQualifiedName));
        }

        [Test]
        public void IdentityTypeSetsModelIdTypePropertyToTypeName()
        {
            Any<SecondMappedObject>()
                .Mapping(m => m
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col")
                    .EntityTypeColumn("col2"))
                .ModelShouldMatch(x => x.IdType.ShouldEqual(typeof(int).AssemblyQualifiedName));
        }

        [Test]
        public void InsertSetsModelInsertPropertyToTrue()
        {
            Any<SecondMappedObject>()
                .Mapping(m => m
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col")
                    .EntityTypeColumn("col2")
                    .Insert())
                .ModelShouldMatch(x => x.Insert.ShouldBeTrue());
        }

        [Test]
        public void NotInsertSetsModelInsertPropertyToFalse()
        {
            Any<SecondMappedObject>()
                .Mapping(m => m
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col")
                    .EntityTypeColumn("col2")
                    .Not.Insert())
                .ModelShouldMatch(x => x.Insert.ShouldBeFalse());
        }

        [Test]
        public void UpdateSetsModelUpdatePropertyToTrue()
        {
            Any<SecondMappedObject>()
                .Mapping(m => m
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col")
                    .EntityTypeColumn("col2")
                    .Update())
                .ModelShouldMatch(x => x.Update.ShouldBeTrue());
        }

        [Test]
        public void NotUpdateSetsModelUpdatePropertyToFalse()
        {
            Any<SecondMappedObject>()
                .Mapping(m => m
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col")
                    .EntityTypeColumn("col2")
                    .Not.Update())
                .ModelShouldMatch(x => x.Update.ShouldBeFalse());
        }

        [Test]
        public void ReadOnlySetsModelInsertPropertyToFalse()
        {
            Any<SecondMappedObject>()
                .Mapping(m => m
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col")
                    .EntityTypeColumn("col2")
                    .ReadOnly())
                .ModelShouldMatch(x => x.Insert.ShouldBeFalse());
        }

        [Test]
        public void NotReadOnlySetsModelInsertPropertyToTrue()
        {
            Any<SecondMappedObject>()
                .Mapping(m => m
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col")
                    .EntityTypeColumn("col2")
                    .Not.ReadOnly())
                .ModelShouldMatch(x => x.Insert.ShouldBeTrue());
        }

        [Test]
        public void ReadOnlySetsModelUpdatePropertyToFalse()
        {
            Any<SecondMappedObject>()
                .Mapping(m => m
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col")
                    .EntityTypeColumn("col2")
                    .ReadOnly())
                .ModelShouldMatch(x => x.Update.ShouldBeFalse());
        }

        [Test]
        public void NotReadOnlySetsModelUpdatePropertyToTrue()
        {
            Any<SecondMappedObject>()
                .Mapping(m => m
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col")
                    .EntityTypeColumn("col2")
                    .Not.ReadOnly())
                .ModelShouldMatch(x => x.Update.ShouldBeTrue());
        }

        [Test]
        public void LazyLoadSetsModelLazyPropertyToTrue()
        {
            Any<SecondMappedObject>()
                .Mapping(m => m
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col")
                    .EntityTypeColumn("col2")
                    .LazyLoad())
                .ModelShouldMatch(x => x.Lazy.ShouldBeTrue());
        }

        [Test]
        public void NotLazyLoadSetsModelLazyPropertyToFalse()
        {
            Any<SecondMappedObject>()
                .Mapping(m => m
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col")
                    .EntityTypeColumn("col2")
                    .Not.LazyLoad())
                .ModelShouldMatch(x => x.Lazy.ShouldBeFalse());
        }

        [Test]
        public void OptimisticLockSetsModelOptimisticLockPropertyToTrue()
        {
            Any<SecondMappedObject>()
                .Mapping(m => m
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col")
                    .EntityTypeColumn("col2")
                    .OptimisticLock())
                .ModelShouldMatch(x => x.OptimisticLock.ShouldBeTrue());
        }

        [Test]
        public void NotOptimisticLockSetsModelOptimisticLockPropertyToFalse()
        {
            Any<SecondMappedObject>()
                .Mapping(m => m
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col")
                    .EntityTypeColumn("col2")
                    .Not.OptimisticLock())
                .ModelShouldMatch(x => x.OptimisticLock.ShouldBeFalse());
        }

        [Test]
        public void MetaTypePropertyShouldBeSetToPropertyTypeIfNoMetaValuesSet()
        {
            Any<SecondMappedObject>()
                .Mapping(m => m
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col")
                    .EntityTypeColumn("col2"))
                .ModelShouldMatch(x => x.MetaType.ShouldEqual(new TypeReference(typeof(SecondMappedObject))));
        }

        [Test]
        public void MetaTypePropertyShouldBeSetToStringIfMetaValuesSet()
        {
            Any<SecondMappedObject>()
                .Mapping(m => m
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col")
                    .EntityTypeColumn("col2")
                    .AddMetaValue<Record>("Rec"))
                .ModelShouldMatch(x => x.MetaType.ShouldEqual(new TypeReference(typeof(string))));
        }

        [Test]
        public void NamePropertyShouldBeSetToPropertyName()
        {
            Any<SecondMappedObject>()
                .Mapping(m => m
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col")
                    .EntityTypeColumn("col2"))
                .ModelShouldMatch(x => x.Name.ShouldEqual("Parent"));
        }

        [Test]
        public void EntityIdentifierColumnShouldAddToModelColumnsCollection()
        {
            Any<SecondMappedObject>()
                .Mapping(m => m
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col")
                    .EntityTypeColumn("col2"))
                .ModelShouldMatch(x => x.IdentifierColumns.Count().ShouldEqual(1));
        }

        [Test]
        public void EntityTypeColumnShouldAddToModelColumnsCollection()
        {
            Any<SecondMappedObject>()
                .Mapping(m => m
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col")
                    .EntityTypeColumn("col2"))
                .ModelShouldMatch(x => x.TypeColumns.Count().ShouldEqual(1));
        }

        [Test]
        public void AddMetaValueShouldAddToModelMetaValuesCollection()
        {
            Any<SecondMappedObject>()
                .Mapping(m => m
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col")
                    .EntityTypeColumn("col2")
                    .AddMetaValue<Record>("Rec"))
                .ModelShouldMatch(x => x.MetaValues.Count().ShouldEqual(1));
        }
    }
}