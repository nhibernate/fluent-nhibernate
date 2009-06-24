//using FluentNHibernate.AutoMap;
//using FluentNHibernate.AutoMap.TestFixtures;
//using NHibernate.Cfg;
//using NUnit.Framework;
//using Rhino.Mocks;
//
//namespace FluentNHibernate.Testing.AutoMap
//{
//    [TestFixture]
//    public class AutoPersistenceModelTests
//    {
//        [Test]
//        public void ShouldSetAssembly()
//        {
//            var autoMapper = AutoPersistenceModel
//                .MapEntitiesFromAssemblyOf<ExampleClass>();
//
//            Assert.AreEqual(autoMapper.Assembly, typeof(ExampleClass).Assembly);
//        }
//
//        [Test]
//        public void ShouldFindTypes()
//        {
//            var autoMapper = AutoPersistenceModel
//                .MapEntitiesFromAssemblyOf<ExampleClass>()
//                .Where(t => t == typeof(ExampleClass));
//
//            Assert.IsNotNull(autoMapper.ClassesFound);
//            Assert.AreEqual(typeof(ExampleClass), autoMapper.ClassesFound[0].Type);
//        }
//
//        [Test]
//        public void ShouldAutoMapType()
//        {
//            var autoMapper1 = MockRepository.GenerateMock<IAutoMapper>();
//            var autoMapper2 = MockRepository.GenerateMock<IAutoMapper>();
//
//            var model = new AutoPersistenceModel(typeof(ExampleClass), new[] { autoMapper1, autoMapper2 })
//                .Where(t => t == typeof (ExampleClass));
//
//            
//            autoMapper1.Expect(m => m.Map(model.ClassesFound[0]));
//            autoMapper2.Expect(m => m.Map(model.ClassesFound[0]));
//
//            model.OutputXml();
//
//            autoMapper1.VerifyAllExpectations();
//            autoMapper2.VerifyAllExpectations();
//        }
//
//        [Test]
//        public void ShouldMergeClassMapsTogether()
//        {
//            var autoMapper = AutoPersistenceModel
//                .MapEntitiesFromAssemblyOf<ExampleClass>()
//                .Where(t => t == typeof (ExampleClass))
//                .ForTypesThatDeriveFrom<ExampleClass>(q => q.Id(p => p.ExampleClassId));
//
//            Assert.IsNotNull(autoMapper.ClassesFound[0].Id); 
//        }
//    }
//}