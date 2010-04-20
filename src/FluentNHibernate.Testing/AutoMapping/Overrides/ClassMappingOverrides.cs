using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Overrides
{
    [TestFixture]
    public class ClassMappingOverrides
    {
        [Test]
        public void CanSetStoredProcedureInOverride()
        {
            var model = AutoMap.Source(new StubTypeSource(new[] { typeof(Parent) }))
               .Override<Parent>(o => o.SqlInsert("EXEC InsertParent"));

            HibernateMapping hibernateMapping = model.BuildMappings().First();

            ClassMapping classMapping = hibernateMapping.Classes.First();
            classMapping.StoredProcedures.ShouldHaveCount(1);
            classMapping.StoredProcedures.First().Query.ShouldEqual("EXEC InsertParent");
        }

        [Test]
        public void CanSetTuplizerInOverride()
        {
            Type tuplizerType = typeof(NHibernate.Tuple.Entity.PocoEntityTuplizer);

            var model = AutoMap.Source(new StubTypeSource(new[] { typeof(Parent) }))
                .Override<Parent>(o => o.Tuplizer(TuplizerMode.Poco, tuplizerType));

            HibernateMapping hibernateMapping = model.BuildMappings().First();
            ClassMapping classMapping = hibernateMapping.Classes.First();
            classMapping.Tuplizer.ShouldNotBeNull();            
        }

        [Test]
        public void CanSetFilterInOverride()
        {
            var model = AutoMap.Source(new StubTypeSource(new[] { typeof(Parent) }))
               .Override<Parent>(o => o.ApplyFilter("filter1"));

            HibernateMapping hibernateMapping = model.BuildMappings().First();

            ClassMapping classMapping = hibernateMapping.Classes.First();
            classMapping.Filters.Single().Name.ShouldEqual("filter1");
        }

    }
}
