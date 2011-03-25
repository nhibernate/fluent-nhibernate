using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Visitors;
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
            var joinedSubclass = new SubclassMapping(SubclassType.JoinedSubclass);
            _classMapping.AddSubclass(joinedSubclass);
            _classMapping.Subclasses.ShouldContain(joinedSubclass);
        }

        [Test]
        public void Should_pass_subclasses_to_the_visitor()
        {
            var classMap = new ClassMapping {Name = "class1" };
            classMap.AddSubclass(new SubclassMapping(SubclassType.JoinedSubclass));

            var visitor = MockRepository.GenerateMock<IMappingModelVisitor>();
            visitor.Expect(x => x.Visit(classMap.Subclasses.First()));

            classMap.AcceptVisitor(visitor);

            visitor.VerifyAllExpectations();           
        }

        [Test]
        public void Can_add_stored_procedure()
        {
            var storedProcedure = new StoredProcedureMapping();
            _classMapping.AddStoredProcedure(storedProcedure);
            _classMapping.StoredProcedures.ShouldContain(storedProcedure);
        }

        [Test]
        public void Should_pass_stored_procedure_to_the_visitor()
        {
            var classMap = new ClassMapping { Name = "class1" };
            classMap.AddStoredProcedure(new StoredProcedureMapping());

            var visitor = MockRepository.GenerateMock<IMappingModelVisitor>();
            visitor.Expect(x => x.Visit(classMap.StoredProcedures.First()));

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