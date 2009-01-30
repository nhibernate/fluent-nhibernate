using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmJoinedSubclassWriterTester
    {
        [Test]
        public void Should_produce_valid_hbm()
        {
            var mapping = new JoinedSubclassMapping { Name = "joinedsubclass1" };
            //var generatorWriter = MockRepository.GenerateStub<IHbmWriter<JoinedSubclassMapping>>();
            //generatorWriter.Expect(x => x.Write(id.Generator)).Return(new HbmGenerator { @class = "native" });
            var writer = new HbmJoinedSubclassWriter();

            writer.ShouldGenerateValidOutput(mapping);
        }
    }
}
