using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.MappingModel
{
    [TestFixture]
    public class ClassMappingTester
    {
        private ClassMapping _classMapping;

        [SetUp]
        public void SetUp()
        {
            _classMapping = new ClassMapping();
        }

        [Test]
        public void CanSetIdToBeStandardIdMapping()
        {
            var idMapping = new IdMapping();
            _classMapping.Id = idMapping;

            _classMapping.Id.ShouldEqual(idMapping);
        }

        [Test]
        public void CanSetIdToBeCompositeIdMapping()
        {
            var idMapping = new CompositeIdMapping();
            _classMapping.Id = idMapping;

            _classMapping.Id.ShouldEqual(idMapping);
        }

        [Test]
        public void CanAddProperty()
        {
            var property = new PropertyMapping() { Name = "Property1" };
            _classMapping.AddProperty(property);

            _classMapping.Properties.ShouldContain(property);
        }

        [Test]
        public void CanAddBag()
        {
            var bag = new BagMapping
                          {
                              Name = "bag1",
                              Key = new KeyMapping(),
                              Relationship = new OneToManyMapping { Class = new TypeReference("class1") }
                          };
            _classMapping.AddCollection(bag);

            _classMapping.Collections.ShouldContain(bag);
        }

        [Test]
        public void CanAddReference()
        {
            var reference = new ManyToOneMapping { Name = "parent" };
            _classMapping.AddReference(reference);

            _classMapping.References.ShouldContain(reference);
        }

        [Test]
        public void Should_pass_id_to_the_visitor()
        {
            var classMap = new ClassMapping {Name = "class1" };
            classMap.Id = new IdMapping();

            var visitor = MockRepository.GenerateMock<IMappingModelVisitor>();
            visitor.Expect(x => x.Visit(classMap.Id));

            classMap.AcceptVisitor(visitor);

            visitor.VerifyAllExpectations();
        }

        [Test]
        public void Should_not_pass_null_id_to_the_visitor()
        {
            var classMap = new ClassMapping {Name = "class1" };
            classMap.Id = null;

            var visitor = MockRepository.GenerateMock<IMappingModelVisitor>();            
            visitor.Expect(x => x.Visit(classMap.Id)).Repeat.Never();            
            
            classMap.AcceptVisitor(visitor);
            
            visitor.VerifyAllExpectations();
        }

        [Test]
        public void Can_add_subclass()
        {
            var joinedSubclass = new JoinedSubclassMapping();
            _classMapping.AddSubclass(joinedSubclass);
            _classMapping.Subclasses.ShouldContain(joinedSubclass);
        }

        [Test]
        public void Should_pass_subclasses_to_the_visitor()
        {
            var classMap = new ClassMapping {Name = "class1" };
            classMap.AddSubclass(new JoinedSubclassMapping());

            var visitor = MockRepository.GenerateMock<IMappingModelVisitor>();
            visitor.Expect(x => x.Visit(classMap.Subclasses.First()));

            classMap.AcceptVisitor(visitor);

            visitor.VerifyAllExpectations();           
        }

        [Test]
        public void Should_pass_the_discriminator_to_the_visitor()
        {
            var classMap = new ClassMapping {Name = "class1" };
            classMap.Discriminator = new DiscriminatorMapping();

            var visitor = MockRepository.GenerateMock<IMappingModelVisitor>();
            visitor.Expect(x => x.Visit(classMap.Discriminator));

            classMap.AcceptVisitor(visitor);

            visitor.VerifyAllExpectations();     
        }
    }
}