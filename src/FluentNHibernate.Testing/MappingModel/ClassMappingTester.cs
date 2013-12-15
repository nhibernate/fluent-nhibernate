using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Visitors;
using FakeItEasy;
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
        public void CanAddReference()
        {
            var reference = new ManyToOneMapping();
            reference.Set(x => x.Name, Layer.Defaults, "parent");
            mapping.AddReference(reference);

            mapping.References.ShouldContain(reference);
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
        public void Can_add_subclass()
        {
            var joinedSubclass = new SubclassMapping(SubclassType.JoinedSubclass);
            mapping.AddSubclass(joinedSubclass);
            mapping.Subclasses.ShouldContain(joinedSubclass);
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
        public void Can_add_stored_procedure()
        {
            var storedProcedure = new StoredProcedureMapping();
            mapping.AddStoredProcedure(storedProcedure);
            mapping.StoredProcedures.ShouldContain(storedProcedure);
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
    }
}