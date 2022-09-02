using System;
using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Visitors;
using FakeItEasy;
using FluentNHibernate.MappingModel.Collections;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel
{
    [TestFixture]
    public class ClassMappingTester
    {
        private ClassMapping mapping;

        [SetUp]
        public void SetUp()
        {
            mapping = new ClassMapping();
        }

        [Test]
        public void CanSetIdToBeStandardIdMapping()
        {
            var idMapping = new IdMapping();
            mapping.Set(x => x.Id, Layer.Defaults, idMapping);

            mapping.Id.ShouldEqual(idMapping);
        }

        [Test]
        public void CanSetIdToBeCompositeIdMapping()
        {
            var idMapping = new CompositeIdMapping();
            mapping.Set(x => x.Id, Layer.Defaults, idMapping);

            mapping.Id.ShouldEqual(idMapping);
        }

        [Test]
        public void CanAddProperty()
        {
            var property = new PropertyMapping();
            property.Set(x => x.Name, Layer.Defaults, "Property1");
            mapping.AddProperty(property);

            mapping.Properties.ShouldContain(property);
        }

        [Test]
        public void ShouldFailToAddDuplicateProperty()
        {
            var property1 = new PropertyMapping();
            property1.Set(x => x.Name, Layer.Defaults, "Property1");
            mapping.AddProperty(property1);

            var property2 = new PropertyMapping();
            property2.Set(x => x.Name, Layer.Defaults, property1.Name);
            Assert.Throws<InvalidOperationException>(() => mapping.AddProperty(property2));
            mapping.Properties.ShouldContain(property1);
        }

        [Test]
        public void CanAddReference()
        {
            var reference = new ManyToOneMapping();
            reference.Set(x => x.Name, Layer.Defaults, "parent");
            mapping.AddReference(reference);

            mapping.References.ShouldContain(reference);
        }

        [Test]
        public void ShouldFailToAddDuplicateReference()
        {
            var reference1 = new ManyToOneMapping();
            reference1.Set(x => x.Name, Layer.Defaults, "parent");
            mapping.AddReference(reference1);

            var reference2 = new ManyToOneMapping();
            reference2.Set(x => x.Name, Layer.Defaults, reference1.Name);
            Assert.Throws<InvalidOperationException>(() => mapping.AddReference(reference2));
            mapping.References.ShouldContain(reference1);
        }

        [Test]
        public void Should_pass_id_to_the_visitor()
        {
            var classMap = new ClassMapping();
            classMap.Set(x => x.Name, Layer.Defaults, "class1");
            classMap.Set(x => x.Id, Layer.Defaults, new IdMapping());

            var visitor = A.Fake<IMappingModelVisitor>();

            classMap.AcceptVisitor(visitor);

            A.CallTo(() => visitor.Visit(classMap.Id)).MustHaveHappened();
        }

        [Test]
        public void Should_not_pass_null_id_to_the_visitor()
        {
            var classMap = new ClassMapping();
            classMap.Set(x => x.Name, Layer.Defaults, "class1");
            classMap.Set(x => x.Id, Layer.Defaults, null);

            var visitor = A.Fake<IMappingModelVisitor>();

            classMap.AcceptVisitor(visitor);

            A.CallTo(() => visitor.Visit(classMap.Id)).MustNotHaveHappened();
        }

        [Test]
        public void CanAddSubclass()
        {
            var subclass = new SubclassMapping(SubclassType.JoinedSubclass);
            mapping.AddSubclass(subclass);

            mapping.Subclasses.ShouldContain(subclass);
        }

        [Test]
        public void Should_pass_subclasses_to_the_visitor()
        {
            // FakeItEasy ;for some reason; calls ToString method on SubClassMapping
            // which ended in NullPointerException if AttributeStore didn't contain Type attribute.
            var attributeStore = new AttributeStore();
            attributeStore.Set("Type", 0, typeof(object));

            var classMap = new ClassMapping();
            classMap.Set(x => x.Name, Layer.Defaults, "class1");
            classMap.AddSubclass(new SubclassMapping(SubclassType.JoinedSubclass, attributeStore));

            var visitor = A.Fake<IMappingModelVisitor>();

            classMap.AcceptVisitor(visitor);

            A.CallTo(() => visitor.Visit(classMap.Subclasses.First())).MustHaveHappened();
        }

        [Test]
        public void CanAddStoredProcedure()
        {
            var storedProcedure = new StoredProcedureMapping();
            mapping.AddStoredProcedure(storedProcedure);

            mapping.StoredProcedures.ShouldContain(storedProcedure);
        }

        [Test]
        public void CanAddDuplicateStoredProcedure()
        {
            // Unlike other types, stored procedures do allow duplicate entries. However, in order to distinguish
            // whether two entries are identical or not, we need to set some non-identity attribute on them to be
            // a different value. For this test, "Query" is easy enough.
            var storedProcedure1 = new StoredProcedureMapping();
            storedProcedure1.Set(x => x.Name, Layer.Defaults, "storedProcedure1");
            storedProcedure1.Set(x => x.Query, Layer.Defaults, "x=y");
            mapping.AddStoredProcedure(storedProcedure1);

            var storedProcedure2 = new StoredProcedureMapping();
            storedProcedure1.Set(x => x.Name, Layer.Defaults, storedProcedure1.Name);
            storedProcedure1.Set(x => x.Query, Layer.Defaults, "x=y");

            // Check that adding the duplicate does _not_ throw
            mapping.AddStoredProcedure(storedProcedure2);

            // Now check that both are present in stored procedures tracker
            mapping.StoredProcedures.ShouldContain(storedProcedure1);
            mapping.StoredProcedures.ShouldContain(storedProcedure2);
        }

        [Test]
        public void Should_pass_stored_procedure_to_the_visitor()
        {
            var classMap = new ClassMapping();
            classMap.Set(x => x.Name, Layer.Defaults, "class1");
            classMap.AddStoredProcedure(new StoredProcedureMapping());

            var visitor = A.Fake<IMappingModelVisitor>();

            classMap.AcceptVisitor(visitor);

            A.CallTo(() => visitor.Visit(classMap.StoredProcedures.First())).MustHaveHappened();
        }

        [Test]
        public void Should_pass_the_discriminator_to_the_visitor()
        {
            var classMap = new ClassMapping();
            classMap.Set(x => x.Name, Layer.Defaults, "class1");
            classMap.Set(x => x.Discriminator, Layer.Defaults, new DiscriminatorMapping());

            var visitor = A.Fake<IMappingModelVisitor>();

            classMap.AcceptVisitor(visitor);

            A.CallTo(() => visitor.Visit(classMap.Discriminator)).MustHaveHappened();
        }

        [Test]
        public void CanAddCollection()
        {
            var collection = CollectionMapping.Bag();
            collection.Set(x => x.Name, Layer.Defaults, "Collection1");
            mapping.AddCollection(collection);

            mapping.Collections.ShouldContain(collection);
        }

        [Test]
        public void ShouldFailToAddDuplicateCollection()
        {
            var collection1 = CollectionMapping.Bag();
            collection1.Set(x => x.Name, Layer.Defaults, "Collection1");
            mapping.AddCollection(collection1);

            var collection2 = CollectionMapping.Bag();
            collection2.Set(x => x.Name, Layer.Defaults, collection1.Name);
            Assert.Throws<InvalidOperationException>(() => mapping.AddCollection(collection2));
            mapping.Collections.ShouldContain(collection1);
        }

        [Test]
        public void CanAddComponent()
        {
            var component = new ComponentMapping(ComponentType.Component);
            component.Set(x => x.Name, Layer.Defaults, "Component1");
            mapping.AddComponent(component);

            mapping.Components.ShouldContain(component);
        }

        [Test]
        public void ShouldFailToAddDuplicateComponent()
        {
            var component1 = new ComponentMapping(ComponentType.Component);
            component1.Set(x => x.Name, Layer.Defaults, "Component1");
            mapping.AddComponent(component1);

            var component2 = new ComponentMapping(ComponentType.Component);
            component2.Set(x => x.Name, Layer.Defaults, component1.Name);
            Assert.Throws<InvalidOperationException>(() => mapping.AddComponent(component2));
            mapping.Components.ShouldContain(component1);
        }

        [Test]
        public void CanAddOneToOne()
        {
            var oneToOne = new OneToOneMapping();
            oneToOne.Set(x => x.Name, Layer.Defaults, "OneToOne1");
            mapping.AddOneToOne(oneToOne);

            mapping.OneToOnes.ShouldContain(oneToOne);
        }

        [Test]
        public void ShouldFailToAddDuplicateOneToOne()
        {
            var oneToOne1 = new OneToOneMapping();
            oneToOne1.Set(x => x.Name, Layer.Defaults, "OneToOne1");
            mapping.AddOneToOne(oneToOne1);

            var oneToOne2 = new OneToOneMapping();
            oneToOne2.Set(x => x.Name, Layer.Defaults, oneToOne1.Name);
            Assert.Throws<InvalidOperationException>(() => mapping.AddOneToOne(oneToOne2));
            mapping.OneToOnes.ShouldContain(oneToOne1);
        }

        [Test]
        public void CanAddAny()
        {
            var any = new AnyMapping();
            any.Set(x => x.Name, Layer.Defaults, "Any1");
            mapping.AddAny(any);

            mapping.Anys.ShouldContain(any);
        }

        [Test]
        public void ShouldFailToAddDuplicateAny()
        {
            var any1 = new AnyMapping();
            any1.Set(x => x.Name, Layer.Defaults, "Any1");
            mapping.AddAny(any1);

            var any2 = new AnyMapping();
            any2.Set(x => x.Name, Layer.Defaults, any1.Name);
            Assert.Throws<InvalidOperationException>(() => mapping.AddAny(any2));
            mapping.Anys.ShouldContain(any1);
        }

        [Test]
        public void CanAddJoin()
        {
            var join = new JoinMapping();
            join.Set(x => x.TableName, Layer.Defaults, "TableName1");
            mapping.AddJoin(join);

            mapping.Joins.ShouldContain(join);
        }

        [Test]
        public void ShouldFailToAddDuplicateJoin()
        {
            var join1 = new JoinMapping();
            join1.Set(x => x.TableName, Layer.Defaults, "TableName1");
            mapping.AddJoin(join1);

            var join2 = new JoinMapping();
            join2.Set(x => x.TableName, Layer.Defaults, join1.TableName);
            Assert.Throws<InvalidOperationException>(() => mapping.AddJoin(join2));
            mapping.Joins.ShouldContain(join1);
        }

        [Test]
        public void CanAddFilter()
        {
            var filter = new FilterMapping();
            filter.Set(x => x.Name, Layer.Defaults, "Filter1");
            mapping.AddFilter(filter);

            mapping.Filters.ShouldContain(filter);
        }

        [Test]
        public void ShouldFailToAddDuplicateFilter()
        {
            var filter1 = new FilterMapping();
            filter1.Set(x => x.Name, Layer.Defaults, "Filter1");
            mapping.AddFilter(filter1);

            var filter2 = new FilterMapping();
            filter2.Set(x => x.Name, Layer.Defaults, filter1.Name);
            Assert.Throws<InvalidOperationException>(() => mapping.AddFilter(filter2));
            mapping.Filters.ShouldContain(filter1);
        }
    }
}