using System.Linq;
using FakeItEasy;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Diagnostics;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Testing.Fixtures;
using NHibernate.Cfg;
using NUnit.Framework;
using static FluentNHibernate.Testing.Cfg.SQLiteFrameworkConfigurationFactory;

namespace FluentNHibernate.Testing.Cfg;

[TestFixture]
public class MappingConfigurationTests
{
    Configuration cfg;
    MappingConfiguration mapping;
    IDiagnosticLogger logger;

    [SetUp]
    public void CreateMappingConfiguration()
    {
        logger = A.Fake<IDiagnosticLogger>();
        cfg = new Configuration();

        CreateStandardInMemoryConfiguration()
            .ConfigureProperties(cfg);

        mapping = new MappingConfiguration(logger);
    }

    [Test]
    public void AddAutoMappingAddsAnyAutoMappedMappingsToCfg()
    {
        mapping.AutoMappings.Add(AutoMap.Source(new StubTypeSource(typeof(Record))));
        mapping.Apply(cfg);

        cfg.ClassMappings.Count.ShouldBeGreaterThan(0);
        cfg.ClassMappings.ShouldContain(c => c.MappedClass == typeof(Record));
    }

    [Test]
    public void AddAutoMappingAddsAnyAutoMappedMappingsToCfgWhenMerged()
    {
        mapping.MergeMappings();
        mapping.AutoMappings.Add(AutoMap.Source(new StubTypeSource(typeof(Record))));
        mapping.Apply(cfg);

        cfg.ClassMappings.Count.ShouldBeGreaterThan(0);
        cfg.ClassMappings.ShouldContain(c => c.MappedClass == typeof(Record));
    }

    [Test]
    public void AddHbmMappingsAddsClasses()
    {
        mapping.HbmMappings.AddClasses(typeof(HbmOne), typeof(HbmTwo));
        mapping.Apply(cfg);

        cfg.ClassMappings.Count.ShouldEqual(2);
        cfg.ClassMappings.ShouldContain(c => c.MappedClass == typeof(HbmOne));
        cfg.ClassMappings.ShouldContain(c => c.MappedClass == typeof(HbmTwo));
    }

    [Test]
    public void AddHbmMappingsFromAssemblyOfAddsClasses()
    {
        mapping.HbmMappings.AddFromAssemblyOf<HbmOne>();
        mapping.Apply(cfg);

        cfg.ClassMappings.Count.ShouldEqual(2);
        cfg.ClassMappings.ShouldContain(c => c.MappedClass == typeof(HbmOne));
        cfg.ClassMappings.ShouldContain(c => c.MappedClass == typeof(HbmTwo));
    }

    [Test]
    public void AddHbmMappingsFromAssemblyAddsClasses()
    {
        mapping.HbmMappings.AddFromAssembly(typeof(HbmOne).Assembly);
        mapping.Apply(cfg);

        cfg.ClassMappings.Count.ShouldEqual(2);
        cfg.ClassMappings.ShouldContain(c => c.MappedClass == typeof(HbmOne));
        cfg.ClassMappings.ShouldContain(c => c.MappedClass == typeof(HbmTwo));
    }

    [Test]
    public void AlteringConventionsShouldAffectProducedClasses()
    {
        mapping.FluentMappings
            .Add<BinaryRecordMap>()
            .Conventions.Add(
                ConventionBuilder.Class.Always(x => x.Table(x.EntityType.Name + "Table"))
            );
        mapping.Apply(cfg);

        cfg.ClassMappings.ShouldContain(c => c.Table.Name == "BinaryRecordTable");
    }

    [Test]
    public void CanAddMultipleConventions()
    {
        mapping.FluentMappings
            .Add<BinaryRecordMap>()
            .Conventions.Add(
                ConventionBuilder.Class.Always(x => x.Table(x.EntityType.Name + "Table")),
                ConventionBuilder.Class.Always(x => x.DynamicInsert())
            );
        mapping.Apply(cfg);

        cfg.ClassMappings.ShouldContain(c => c.Table.Name == "BinaryRecordTable" && c.DynamicInsert == true);
    }

    [Test]
    public void WasUsedIsFalseWhenNothingCalled()
    {
        mapping.WasUsed.ShouldBeFalse();
    }

    [Test]
    public void WasUsedIsTrueWhenAddFromAssemblyOfCalled()
    {
        mapping.FluentMappings.AddFromAssemblyOf<Record>();
        mapping.WasUsed.ShouldBeTrue();
    }

    [Test]
    public void WasUsedIsTrueWhenAddFromAssemblyCalled()
    {
        mapping.FluentMappings.AddFromAssembly(typeof(Record).Assembly);
        mapping.WasUsed.ShouldBeTrue();
    }

    [Test]
    public void WasUsedIsTrueWhenAddAutoMappingsCalled()
    {
        mapping.AutoMappings.Add(AutoMap.Source(new StubTypeSource(typeof(Record))));
        mapping.WasUsed.ShouldBeTrue();
    }

    [Test]
    public void WasUsedIsTrueWhenAddHbmMappingsCalled()
    {
        mapping.HbmMappings.AddClasses(typeof(HbmOne));
        mapping.WasUsed.ShouldBeTrue();
    }

    [Test]
    public void WasUsedIsTrueWhenAddHbmMappingsFromAssemblyCalled()
    {
        mapping.HbmMappings.AddFromAssembly(typeof(HbmOne).Assembly);
        mapping.WasUsed.ShouldBeTrue();
    }

    [Test]
    public void WasUsedIsTrueWhenAddHbmMappingsFromAssemblyOfCalled()
    {
        mapping.HbmMappings.AddFromAssemblyOf<HbmOne>();
        mapping.WasUsed.ShouldBeTrue();
    }

    [Test]
    public void MergeOutputShouldSetFlagOnAutoPersistenceModels()
    {
        mapping.AutoMappings.Add(AutoMap.Source(new EmptySource()));
        mapping.MergeMappings();
        mapping.Apply(new Configuration());
        mapping.AutoMappings.First().MergeMappings.ShouldBeTrue();
    }

    [Test]
    public void MergeOutputShouldSetFlagOnFluentPersistenceModelsOnApply()
    {
        var model = new PersistenceModel();

        mapping.UsePersistenceModel(model);
        mapping.MergeMappings();
        mapping.Apply(new Configuration());

        model.MergeMappings.ShouldBeTrue();
    }
}
