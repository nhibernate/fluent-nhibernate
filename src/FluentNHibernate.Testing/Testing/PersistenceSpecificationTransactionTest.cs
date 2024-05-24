using System.Collections;
using FakeItEasy;
using NHibernate;
using NHibernate.Engine;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Testing;

[TestFixture]
public class PersistenceSpecificationTransactionTest
{
    class Cat
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public CatType CatType { get; set; }
    }

    class CatType
    {
        public string Name { get; set; }
    }

    [Test]
    public void Should_Not_Open_A_New_Transaction_If_One_Is_Passed()
    {
        var sessionImpl = A.Fake<ISessionImplementor>();
        A.CallTo(() => sessionImpl.TransactionInProgress).Returns(true);

        var session = A.Fake<ISession>();
        A.CallTo(() => session.GetSessionImplementation()).Returns(sessionImpl);

        var spec = new PersistenceSpecification<Cat>(session);
        spec.VerifyTheMappings();

        A.CallTo(() => session.BeginTransaction()).MustNotHaveHappened();
    }

    [Test]
    public void Should_Open_A_New_Transaction_If_None_Is_Passed()
    {
        var sessionImpl = A.Fake<ISessionImplementor>();
        A.CallTo(() => sessionImpl.TransactionInProgress).Returns(false);

        var session = A.Fake<ISession>();
        A.CallTo(() => session.GetSessionImplementation()).Returns(sessionImpl);
        A.CallTo(() => session.BeginTransaction()).Returns(A.Dummy<ITransaction>());

        var spec = new PersistenceSpecification<Cat>(session);
        spec.VerifyTheMappings();

        A.CallTo(() => session.BeginTransaction()).MustHaveHappened(1, Times.Exactly);
    }

    [Test]
    public void Passed_Transaction_Should_Apply_For_Reference_Saving()
    {
        var sessionImpl = A.Fake<ISessionImplementor>();
        A.CallTo(() => sessionImpl.TransactionInProgress).Returns(true);

        var session = A.Fake<ISession>();
        A.CallTo(() => session.GetSessionImplementation()).Returns(sessionImpl);
        A.CallTo(() => session.BeginTransaction()).Returns(A.Dummy<ITransaction>());

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

        A.CallTo(() => session.BeginTransaction()).MustNotHaveHappened();
    }

    public class NameEqualityComparer : IEqualityComparer
    {
        bool IEqualityComparer.Equals(object x, object y)
        {
            if (x is null || y is null)
                return false;

            if (x.GetType().GetProperty("Name") is { } xProp && y.GetType().GetProperty("Name") is { } yProp)
            {
                return xProp.GetValue(x, null) == yProp.GetValue(x, null);
            }

            return x.Equals(y);
        }

        public int GetHashCode(object obj)
        {
            return 0;
        }
    }


}
