using System;
using FluentNHibernate.Automapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Overrides
{
    public class ReferenceComponentOverrides
    {
        [Test]
        public void should_be_able_to_build_mapping()
        {
            var source = new StubTypeSource(typeof(IncomingDocument), typeof(DocumentNumber));

            var persistenceModel = AutoMap.Source(source, new AutomappingConfiguration())
                .Override<IncomingDocument>(m => m.Component(x => x.IncomingNumber)
                    .ColumnPrefix("INCOMING_"));

            Assert.DoesNotThrow(() => persistenceModel.BuildMappings());
        }

        class AutomappingConfiguration : DefaultAutomappingConfiguration
        {
            public override bool IsComponent(Type type)
            {
                return type == typeof(DocumentNumber);
            }

            public override bool ShouldMap(Type type)
            {
                return type == typeof(IncomingDocument);
            }
        }

        class DocumentNumber
        {}

        class IncomingDocument
        {
            public virtual int Id { get; set; }
            public virtual DocumentNumber IncomingNumber { get; set; }
        }
    }
}