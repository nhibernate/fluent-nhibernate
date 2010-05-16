using System.Linq;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.PersistenceModel.Fixtures;
using Machine.Specifications;

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
}
