using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FluentNHibernate.Generation;

namespace FluentNHibernate.Testing.DomainModel.Generation
{
    [TestFixture]
    public class CodeFileSmokeTester
    {
        [Test]
        public void SmokeTest_Code_File()
        {
            CodeFile file = new CodeFile("Fixtures.cs", "DomainFixtureGeneration");
            file.AddDomainType<Case>();
            file.AddDomainType<Contact>();

            file.WriteToConsole();
            file.WriteToFile();
        }
    }
}
