using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NHibernate.Cfg;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class TablePerHierarchyTests//: BaseManualMapFixture
    {
        //[Test]
        //public void ShouldHonorBatchSizeOnASubclass()
        //{
        //    var model = new PersistenceModel();
        //    model.Add(new BaseMapping());
        //    model.Add(new DerivedMapping());

        //    var mappings = model.BuildMappings();
        //    var subclassMap = mappings.SelectMany(m => m.Classes)
        //        .FirstOrDefault(cm => cm.Subclasses.Any()).Subclasses.FirstOrDefault();
        //    subclassMap.BatchSize.ShouldEqual(858);
        //}

        //[Test]
        //public void ShouldHonorBatchSizeOnASubclass()
        //{
        //    AddClassMapping(new BaseMapping());
        //    Test<Derived>(x => x.ForSubclassMapping(new DerivedMapping()).HasAttribute("batch-size", "858"));
        //}
    }

    public class Base
    {
        public int Id { get; set; }
        public string Discriminator { get; set; }
        public decimal Value { get; set; }
    }

    public class Derived : Base
    {
        public string FirstName { get; set; }
    }

    public class BaseMapping : ClassMap<Base>
    {
        public BaseMapping()
        {
            Id(x => x.Id);
            Map(x => x.Value);
            DiscriminateSubClassesOnColumn("Discriminator");
        }
    }

    public class DerivedMapping : SubclassMap<Derived>
    {
        public DerivedMapping()
        {
            Map(x => x.FirstName);
            BatchSize(858);
        }
    }

    public abstract class BaseManualMapFixture
    {
        private Configuration cfg;
        private PersistenceModel model;

        [SetUp]
        public void CreateDatabaseCfg()
        {
            cfg = new Configuration();
            model = new PersistenceModel();

            SQLiteConfiguration.Standard
                .InMemory()
                .ConfigureProperties(cfg);
        }

        protected void AddClassMapping(IMappingProvider mapping)
        {
            model.Add(mapping);
        }

        protected void AddSubClassMapping(IIndeterminateSubclassMappingProvider mapping)
        {
            model.Add(mapping);
        }

        protected void OutputMappings(StringBuilder builder)
        {
            model.WriteMappingsTo(new StringWriter(builder));
        }

        protected void Test<T>(Action<MappingTester<T>> mappingTester)
        {
            mappingTester(new MappingTester<T>(model));
        }
    }
}
