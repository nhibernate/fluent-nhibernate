using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.MappingModel
{
    [TestFixture]
    public class HibernateMappingTester
    {


        [Test]
        public void CanAddClassMappings()
        {
            var hibMap = new HibernateMapping();
            var classMap1 = new ClassMapping {Name = "class1" };
            var classMap2 = new ClassMapping {Name = "class1" };
            
            hibMap.AddClass(classMap1);
            hibMap.AddClass(classMap2);

            hibMap.Classes.ShouldContain(classMap1);
            hibMap.Classes.ShouldContain(classMap2);
        }

        [Test]
        public void CanSpecifyDefaultLazy()
        {
            var hibMap = new HibernateMapping();
            hibMap.DefaultLazy = true;
            hibMap.DefaultLazy.ShouldBeTrue();
        }

        [Test]
        public void Should_pass_classmappings_to_the_visitor()
        {
            var hibMap = new HibernateMapping();
            var classMap = new ClassMapping {Name = "class1" };
            hibMap.AddClass(classMap);

            var visitor = MockRepository.GenerateMock<IMappingModelVisitor>();
            visitor.Expect(x => x.Visit(classMap));

            hibMap.AcceptVisitor(visitor);

            visitor.VerifyAllExpectations();
        }
        
    }
}