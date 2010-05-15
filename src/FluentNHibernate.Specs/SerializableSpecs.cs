using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;
using Machine.Specifications;

namespace FluentNHibernate.Specs
{
    public class for_serialization_to_occur
    {
        Establish context = () =>
            mapping_types = typeof(IMappingBase).Assembly
                .GetTypes()
                .Where(x => x.HasInterface(typeof(IMappingBase)) && !x.IsInterface);

        Because of = () =>
            unserializable_types = mapping_types
                .Where(x => x.GetCustomAttributes(typeof(SerializableAttribute), false).Length == 0);

        It should_have_all_mapping_types_marked_as_serializable = () =>
            unserializable_types.ShouldBeEmpty();

        static IEnumerable<Type> mapping_types;
        static IEnumerable<Type> unserializable_types;
    }
}
