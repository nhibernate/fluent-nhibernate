using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.Testing.Automapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Apm
{
    [TestFixture]
    public class IdentityTests
    {
        [Test]
        public void CanUseIdentityGeneratorForIntIds()
        {
            var autoMapper = AutoMap.Source(new StubTypeSource(typeof(ClassWithIntId)));

            new AutoMappingTester<ClassWithIntId>(autoMapper)
                .Element("class/id/generator").HasAttribute("class", "identity");
        }

        [Test]
        public void CanUseIdentityGeneratorForLongIds()
        {
            var autoMapper = AutoMap.Source(new StubTypeSource(typeof(ClassWithLongId)));

            new AutoMappingTester<ClassWithLongId>(autoMapper)
                .Element("class/id/generator").HasAttribute("class", "identity");
        }

        [Test]
        public void CanUseIdentityGeneratorForByteIds()
        {
            var autoMapper = AutoMap.Source(new StubTypeSource(typeof(ByteId)));

            new AutoMappingTester<ByteId>(autoMapper)
                .Element("class/id/generator").HasAttribute("class", "identity");
        }

        [Test]
        public void CanUseIdentityGeneratorForShortIds()
        {
            var autoMapper = AutoMap.Source(new StubTypeSource(typeof(ShortId)));

            new AutoMappingTester<ShortId>(autoMapper)
                .Element("class/id/generator").HasAttribute("class", "identity");
        }
    }

    class ClassWithIntId
    {
        public virtual int Id { get; set; }
    }

    class ClassWithLongId
    {
        public virtual long Id { get; set; }
    }

    class ByteId
    {
        public virtual byte Id { get; set; }
    }

    class ShortId
    {
        public virtual short Id { get; set; }
    }
}
