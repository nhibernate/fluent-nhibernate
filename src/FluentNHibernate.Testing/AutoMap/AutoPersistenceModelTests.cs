using FluentNHibernate.FluentInterface.AutoMap;
using NHibernate.Cfg;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.AutoMap
{
    [TestFixture]
    public class AutoPersistenceModelTests
    {
        [Test]
        public void ShouldSetAssembly()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>();

            Assert.AreEqual(autoMapper.Assembly, typeof(ExampleClass).Assembly);
        }

        [Test]
        public void ShouldFindTypes()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t == typeof(ExampleClass));

            Assert.IsNotNull(autoMapper.ClassesFound);
            Assert.AreEqual(typeof(ExampleClass), autoMapper.ClassesFound[0].Type);
        }

        [Test]
        public void ShouldFindTypesWithTwoWheres()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t == typeof(ExampleClass))
                .Where(t => t == typeof(ExampleClass2));

            Assert.IsNotNull(autoMapper.ClassesFound);
            Assert.AreEqual(typeof(ExampleClass), autoMapper.ClassesFound[0].Type);
            Assert.AreEqual(typeof(ExampleClass2), autoMapper.ClassesFound[1].Type);
        }

        [Test]
        public void ShouldAutoMapType()
        {
            var autoMapper1 = MockRepository.GenerateMock<IAutoMapper>();
            var autoMapper2 = MockRepository.GenerateMock<IAutoMapper>();

            var model = new AutoPersistenceModel(typeof(ExampleClass), new[] { autoMapper1, autoMapper2 })
                .Where(t => t == typeof (ExampleClass));

            
            autoMapper1.Expect(m => m.Map(model.ClassesFound[0]));
            autoMapper2.Expect(m => m.Map(model.ClassesFound[0]));

            model.OutputXml();

            autoMapper1.VerifyAllExpectations();
            autoMapper2.VerifyAllExpectations();
        }
    }

    public class ExampleClass
    {
        public int Id { get; set; }
    }
    public class ExampleClass2 {}
}