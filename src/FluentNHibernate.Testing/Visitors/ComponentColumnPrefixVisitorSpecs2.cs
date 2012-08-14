using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Visitors
{
    [TestFixture]
    public class ComponentColumnPrefixVisitorSpecs2: ComponentColumnPrefixVisitorSpec
    {
        PersistenceModel model;
        IEnumerable<HibernateMapping> mappings;
        ClassMapping targetMapping;
        const string columnPrefix = "{property}";

        public override void establish_context()
        {
            model = new PersistenceModel();

            var componentMap = new ComponentMap<FieldComponent>();
            componentMap.Map(x => x.X);
            componentMap.Map(x => x.Y);

            model.Add(componentMap);

            var classMapping = new ClassMap<Root>();
            classMapping.Id(r => r.Id);
            classMapping.Component(Reveal.Member<Root, FieldComponent>("component"), cpt => cpt.Access.Field().ColumnPrefix(columnPrefix));
            model.Add(classMapping);
        }

        public override void because()
        {
            mappings = model.BuildMappings().ToList();
            targetMapping = mappings.SelectMany(x => x.Classes).FirstOrDefault(x => x.Type == typeof(Root));
        }

        [Test]
        public void should_prefix_field_columns()
        {
            var t = targetMapping.Components.Single();
            Console.Write("fdkgndfgkndfgkjn");
                //.Properties.SelectMany(x => x.Columns)
                //.Each(c => c.Name.ShouldStartWith("component"));
        }
    }

    class Root
    {
        FieldComponent component;
        public int Id { get; set; }
    }

    class FieldComponent
    {
        public string X { get; set; }
        public int? Y { get; set; }
    }
}
