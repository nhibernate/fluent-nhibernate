using System.Linq;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests;

[TestFixture]
public class ClassMapFilterPartModelGenerationTests : BaseModelFixture
{
    [Test]
    public void MapShouldAddFilterMappingToClassMapping()
    {
        ClassMap<PropertyTarget>()
            .Mapping(m => m.ApplyFilter<TestFilter>("Name = :name"))
            .ModelShouldMatch(CheckFirstFilterWithCondition);
    }

    void CheckFirstFilterWithoutCondition(ClassMapping mapping)
    {
        if (!mapping.Filters.Any()) Assert.Fail("No filter added");
        if (mapping.Filters.First().Name != "test") Assert.Fail("Wrong filter name added");
    }

    void CheckFirstFilterWithCondition(ClassMapping mapping)
    {
        CheckFirstFilterWithoutCondition(mapping);
        if (mapping.Filters.First().Condition != "Name = :name") Assert.Fail("Wrong filter condition added");
    }

    [Test]
    public void MapShouldAddFilterMappingWithoutConditionToClassMapping()
    {
        ClassMap<PropertyTarget>()
            .Mapping(m => m.ApplyFilter<TestFilter>())
            .ModelShouldMatch(CheckFirstFilterWithoutCondition);
    }
}
