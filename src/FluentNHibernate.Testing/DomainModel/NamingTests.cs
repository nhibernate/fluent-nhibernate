using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel
{
    [TestFixture]
    public class NamingTests
    {
        [Test]
        public void Lowercase()
        {
            Naming.Determine("alllowercase")
                .ShouldEqual(NamingStrategy.LowerCase);
        }

        [Test]
        public void LowercaseUnderscore()
        {
            Naming.Determine("_alllowercase")
                .ShouldEqual(NamingStrategy.LowerCaseUnderscore);
        }

        [Test]
        public void LowercaseMUnderscore()
        {
            Naming.Determine("m_alllowercase")
                .ShouldEqual(NamingStrategy.Unknown);
        }

        [Test]
        public void Camelcase()
        {
            Naming.Determine("camelCase")
                .ShouldEqual(NamingStrategy.CamelCase);
        }

        [Test]
        public void CamelcaseUnderscore()
        {
            Naming.Determine("_camelCase")
                .ShouldEqual(NamingStrategy.CamelCaseUnderscore);
        }

        [Test]
        public void CamelcaseMUnderscore()
        {
            Naming.Determine("m_camelCase")
                .ShouldEqual(NamingStrategy.Unknown);
        }

        [Test]
        public void Pascalcase()
        {
            Naming.Determine("PascalCase")
                .ShouldEqual(NamingStrategy.PascalCase);
        }

        [Test]
        public void PascalcaseM()
        {
            Naming.Determine("mPascalCase")
                .ShouldEqual(NamingStrategy.PascalCaseM);
        }

        [Test]
        public void PascalcaseMUnderscore()
        {
            Naming.Determine("m_PascalCase")
                .ShouldEqual(NamingStrategy.PascalCaseMUnderscore);
        }

        [Test]
        public void PascalcaseUnderscore()
        {
            Naming.Determine("_PascalCase")
                .ShouldEqual(NamingStrategy.PascalCaseUnderscore);
        }
    }
}
