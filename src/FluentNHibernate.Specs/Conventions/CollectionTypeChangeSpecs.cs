using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Specs.Conventions.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Conventions
{
    public class when_changing_the_collection_type_with_conventions
    {
        Establish context = () =>
        {
            model = new FluentNHibernate.PersistenceModel();
            model.Conventions.Add<CollectionConvention>();
            model.Add(new CollectionTargetMap());
            model.Add(new CollectionChildTargetMap());
        };

        Because of = () =>
            mapping = model.BuildMappingFor<CollectionTarget>();

        It should_be_able_to_change_a_bag_to_a_list = () =>
            mapping.Collections
                .Single(x => x.Name == "Bag")
                .Collection.ShouldEqual(Collection.List);

        It should_be_able_to_change_a_set_to_a_list = () =>
            mapping.Collections
                .Single(x => x.Name == "Set")
                .Collection.ShouldEqual(Collection.List);
        
        static FluentNHibernate.PersistenceModel model;
        static ClassMapping mapping;
    }
}
