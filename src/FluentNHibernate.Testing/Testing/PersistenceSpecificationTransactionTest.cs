using System;
using System.Collections;
using FakeItEasy;
using NHibernate;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Testing
{
    [TestFixture]
    public class PersistenceSpecificationTransactionTest
    {
        private class Cat
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public CatType CatType { get; set; }
        }

        private class CatType
        {
            public string Name { get; set; }
        }

        [Test]
        public void Should_Not_Open_A_New_Transaction_If_One_Is_Passed()
        {

            var transaction = A.Fake<ITransaction>();
            A.CallTo(() => transaction.IsActive).Returns(true);

            var session = A.Fake<ISession>();
#pragma warning disable CS0618 // Type or member is obsolete
            A.CallTo(() => session.Transaction).Returns(transaction);
#pragma warning restore CS0618 // Type or member is obsolete

            var spec = new PersistenceSpecification<Cat>(session);
            spec.VerifyTheMappings();

            A.CallTo(() => session.BeginTransaction()).MustNotHaveHappened();
#pragma warning disable CS0618 // Type or member is obsolete
            A.CallTo(() => session.Transaction).MustHaveHappened(1, Times.Exactly);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [Test]
        public void Should_Open_A_New_Transaction_If_None_Is_Passed()
        {

            var transaction = A.Fake<ITransaction>();
            A.CallTo(() => transaction.IsActive).Returns(false);

            var session = A.Fake<ISession>();
#pragma warning disable CS0618 // Type or member is obsolete
            A.CallTo(() => session.Transaction).Returns(transaction);
#pragma warning restore CS0618 // Type or member is obsolete
            A.CallTo(() => session.BeginTransaction()).Returns(transaction);

            var spec = new PersistenceSpecification<Cat>(session);
            spec.VerifyTheMappings();

#pragma warning disable CS0618 // Type or member is obsolete
            A.CallTo(() => session.Transaction).MustHaveHappened(1, Times.Exactly);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [Test]
        public void Passed_Transaction_Should_Apply_For_Reference_Saving()
        {
            var transaction = A.Fake<ITransaction>();
            A.CallTo(() => transaction.IsActive).Returns(true);

            var session = A.Fake<ISession>();
#pragma warning disable CS0618 // Type or member is obsolete
            A.CallTo(() => session.Transaction).Returns(transaction);
#pragma warning restore CS0618 // Type or member is obsolete
            A.CallTo(() => session.Get<Cat>(null)).WithAnyArguments()
                .Returns(
                new Cat
                {
                    Name = "Fluffy",
                    CatType = new CatType
                    {
                        Name = "Persian"
                    }
                });


            var spec = new PersistenceSpecification<Cat>(session, new NameEqualityComparer());
            spec.CheckProperty(x => x.Name, "Fluffy");
            spec.CheckReference(x => x.CatType, new CatType { Name = "Persian" });

            spec.VerifyTheMappings();

#pragma warning disable CS0618 // Type or member is obsolete
            A.CallTo(() => session.Transaction).MustHaveHappened(1, Times.Exactly);
#pragma warning restore CS0618 // Type or member is obsolete
            A.CallTo(() => session.BeginTransaction()).MustNotHaveHappened();
        }

        public class NameEqualityComparer : IEqualityComparer
        {
            bool IEqualityComparer.Equals(object x, object y)
            {
                if (x == null || y == null)
                    return false;

                if (x.GetType().GetProperty("Name") != null && y.GetType().GetProperty("Name") != null)
                {
                    return x.GetType().GetProperty("Name").GetValue(x, null) == y.GetType().GetProperty("Name").GetValue(x, null);
                }

                return x.Equals(y);
            }

            public int GetHashCode(object obj)
            {
                return 0;
            }
        }


    }
}
