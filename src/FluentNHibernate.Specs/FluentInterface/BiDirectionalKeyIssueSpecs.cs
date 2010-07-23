using System.Linq;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.FluentInterface.Fixtures.BiDirectionalKeyIssue;
using Machine.Specifications;

namespace FluentNHibernate.Specs.FluentInterface
{
    public class when_there_are_two_references_to_the_same_entity
    {
        Establish context = () =>
        {
            model = new FluentNHibernate.PersistenceModel();
            model.Add(new ContactMap());
            model.Add(new ContactEmailMap());
            model.Add(new ContactPhoneMap());
            model.Add(new CaseMap());
        };

        Because of = () =>
            contact_mapping = model.BuildMappingFor<Contact>();

        It should_work_like_1_0_did_aka_not_create_multiple_columns_to_the_same_entity = () =>
            contact_mapping.Collections
                .Single(x => x.Name == "EmailAddresses")
                .Key.Columns.Select(x => x.Name).ShouldContainOnly("Contact_id");

        static FluentNHibernate.PersistenceModel model;
        static ClassMapping contact_mapping;
    }
}
