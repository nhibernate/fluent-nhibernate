using FluentNHibernate.Framework.Generation;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Generation
{
    [TestFixture, Explicit]
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
