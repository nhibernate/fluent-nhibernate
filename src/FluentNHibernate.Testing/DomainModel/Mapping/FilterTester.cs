using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class FilterTester
    {
        [Test]
        public void Applying_a_filter()
        {
            new MappingTester<MappedObject>()
               .ForMapping(map => map.ApplyFilter<TestFilter>("Name = :name"))
               .Element("class/filter")
                   .HasAttribute("name", "test")
                   .HasAttribute("condition", "Name = :name");
        }

        [Test]
        public void Applying_a_filter_with_no_condition()
        {
            new MappingTester<MappedObject>()
               .ForMapping(map => map.ApplyFilter<TestFilter>())
               .Element("class/filter")
                   .HasAttribute("name", "test")
                   .DoesntHaveAttribute("condition");
        }

        [Test]
        public void Applying_a_filter_to_a_one_to_many()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(m => m.HasMany(x => x.SetOfChildren)
                    .ApplyFilter<TestFilter>("Name = :name"))
                .Element("class/set/filter")
                .HasAttribute("name", "test")
                .HasAttribute("condition", "Name = :name");
        }

        [Test]
        public void Applying_a_filter_to_a_one_to_many_with_no_condition()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(m => m.HasMany(x => x.SetOfChildren)
                    .ApplyFilter<TestFilter>())
                .Element("class/set/filter")
                .HasAttribute("name", "test")
                .DoesntHaveAttribute("condition");
        }

        [Test]
        public void Applying_a_filter_to_a_many_to_many()
        {
            MappingTester<ManyToManyTarget> mapping = new MappingTester<ManyToManyTarget>()
                .ForMapping(m => m.HasManyToMany(x => x.BagOfChildren)
                    .ApplyFilter<TestFilter>("Name = :name"));
            mapping
                .Element("class/bag/filter")
                .HasAttribute("name", "test")
                .HasAttribute("condition", "Name = :name");
        }

        [Test]
        public void Applying_a_filter_to_a_many_to_many_with_no_condition()
        {
            new MappingTester<ManyToManyTarget>()
                .ForMapping(m => m.HasManyToMany(x => x.BagOfChildren)
                    .ApplyFilter<TestFilter>())
                .Element("class/bag/filter")
                .HasAttribute("name", "test")
                .DoesntHaveAttribute("condition");
        }

    }

    internal class TestFilter : FilterDefinition
    {
        public TestFilter()
        {
            WithName("test").WithCondition("Name = :testName");
        }
    }
}
