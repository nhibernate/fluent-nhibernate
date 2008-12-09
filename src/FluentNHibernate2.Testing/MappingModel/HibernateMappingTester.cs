using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel
{
    [TestFixture]
    public class HibernateMappingTester
    {
        [Test]
        public void CanConstructValidInstance()
        {
            var mapping = new HibernateMapping();
            mapping.ShouldBeValidAgainstSchema();            
        }

        [Test]
        public void CanAddClassMappings()
        {
            var hibMap = new HibernateMapping();
            var classMap1 = MappingMother.CreateClassMapping();
            var classMap2 = MappingMother.CreateClassMapping();
            
            hibMap.AddClass(classMap1);
            hibMap.AddClass(classMap2);

            hibMap.Classes.ShouldContain(classMap1);
            hibMap.Hbm.Items.ShouldContain(classMap1.Hbm);

            hibMap.Classes.ShouldContain(classMap2);
            hibMap.Hbm.Items.ShouldContain(classMap2.Hbm);
        }

        [Test]
        public void CanSpecifyDefaultLazy()
        {
            var hibMap = new HibernateMapping();
            hibMap.DefaultLazy = false;

            hibMap.DefaultLazy.ShouldBeFalse();
            hibMap.Hbm.defaultlazy.ShouldBeFalse();
        }
        
    }
}