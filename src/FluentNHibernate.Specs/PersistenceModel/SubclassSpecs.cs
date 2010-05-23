using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.PersistenceModel.Fixtures;
using Machine.Specifications;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace FluentNHibernate.Specs.PersistenceModel
{
    public class when_subclass_map_is_combined_with_a_class_map_flagged_as_union
    {
        Establish context = () =>
        {
            model = new FluentNHibernate.PersistenceModel();
            model.Add(new UnionEntityMap());
            model.Add(new UnionChildEntityMap());
        };

        Because of = () =>
            mapping = model.BuildMappingFor<UnionEntity>();

        It should_map_the_subclass_as_a_union_subclass = () =>
            mapping.Subclasses.Single().SubclassType.ShouldEqual(SubclassType.UnionSubclass);

        static FluentNHibernate.PersistenceModel model;
        static ClassMapping mapping;
    }

    public class when_subclass_map_has_a_has_many_to_another_entity
    {
        Establish context = () =>
        {
            model = new FluentNHibernate.PersistenceModel();
            model.Add(new ProductMap());
            model.Add(new SpecialProductMap());
            model.Add(new OptionMap());

            cfg = new Configuration();
            SQLiteConfiguration.Standard.InMemory()
                .ConfigureProperties(cfg);
            model.Configure(cfg);

            new SchemaExport(cfg).Create(true, false);
        };

        Because of = () =>
            mappings = model.BuildMappings()
                .SelectMany(x => x.Classes);

        It should_only_use_one_column_in_the_target_entity_s_key = () =>
            mappings.Single(x => x.Type == typeof(Product))
                .Subclasses.Single()
                .Collections.Single()
                .Key.Columns.Select(x => x.Name)
                .ShouldContainOnly("SpecialProduct_id");

        static FluentNHibernate.PersistenceModel model;
        static IEnumerable<ClassMapping> mappings;
        static Configuration cfg;

        class Product
        {
            public int ProductId { get; set; }
        }

        class SpecialProduct : Product
        {
            public ICollection<Option> Options { get; set;}
        }

        class Option
        {
            public int OptionId { get; set; }
            public SpecialProduct Back { get; set; }
        }

        class ProductMap : ClassMap<Product>
        {
            public ProductMap()
            {
                Id(x => x.ProductId);
            }
        }

        class SpecialProductMap : SubclassMap<SpecialProduct>
        {
            public SpecialProductMap()
            {
                Extends<Product>();
                HasMany(x => x.Options).Cascade.AllDeleteOrphan();
            }
        }

        class OptionMap : ClassMap<Option>
        {
            public OptionMap()
            {
                Id(x => x.OptionId);
            }
        }
    }
}
