using System;
using System.Collections;
using NHibernate;
using NUnit.Framework;
using Rhino.Mocks;

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

            var transaction = MockRepository.GenerateMock<ITransaction>();
            transaction.Expect(x => x.IsActive).Return(true);

            var session = MockRepository.GenerateMock<ISession>();
            session.Expect(x => x.Transaction).Return(transaction).Repeat.Twice();
            session.Expect(x => x.BeginTransaction()).Repeat.Never();

            var spec = new PersistenceSpecification<Cat>(session);
            spec.VerifyTheMappings();

            session.VerifyAllExpectations();


        }

        [Test]
        public void Should_Open_A_New_Transaction_If_None_Is_Passed()
        {

            var transaction = MockRepository.GenerateMock<ITransaction>();
            transaction.Expect(x => x.IsActive).Return(false);

            var session = MockRepository.GenerateMock<ISession>();
            session.Expect(x => x.Transaction).Return(transaction).Repeat.Twice();

            session.Expect(x => x.BeginTransaction()).Return(transaction).Repeat.Once();

            var spec = new PersistenceSpecification<Cat>(session);
            spec.VerifyTheMappings();

            session.VerifyAllExpectations();

        }

        [Test]
        public void Passed_Transaction_Should_Apply_For_Reference_Saving()
        {
            var transaction = MockRepository.GenerateMock<ITransaction>();
            transaction.Expect(x => x.IsActive).Return(true);


            var session = MockRepository.GenerateMock<ISession>();
            session.Expect(x => x.Transaction).Return(transaction).Repeat.Twice();

            session.Expect(x => x.BeginTransaction()).Repeat.Never();
            session.Expect(s => s.Get<Cat>(null)).IgnoreArguments().Return(
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
            spec.CheckReference(x => x.CatType, new CatType {Name = "Persian"});
            
            spec.VerifyTheMappings();

            session.VerifyAllExpectations();
            
        }



        public class NameEqualityComparer : IEqualityComparer
        {
            public bool Equals(object x, object y)
            {
                if ( x == null || y == null )
                    return false;

                if ( x.GetType().GetProperty("Name") != null && y.GetType().GetProperty("Name") != null )
                {
                    return x.GetType().GetProperty("Name").GetValue(x, null) == y.GetType().GetProperty("Name").GetValue(x, null);
                }

                return x.Equals(y);
            }

            public int GetHashCode(object obj)
            {
                throw new NotImplementedException();
            }
        }


    }
}
