using FluentNHibernate.QuickStart.Domain;
using FluentNHibernate.QuickStart.Domain.DataAccess;
using NHibernate;
using NUnit.Framework;

namespace FluentNHibernate.QuickStart.Integration.Testing
{
    [TestFixture]
    public class QuickStartTester
    {
        private NHibernateSessionManager _sessionFactory;
        private NHibernateDataProvider _provider;

        #region Setup/Teardown

        [SetUp]
        public void Test_Set_Up()
        {
            _provider = new NHibernateDataProvider(_sessionFactory.GetSession());
        }

        [TestFixtureSetUp]
        public void Test_Fixture_Set_Up()
        {
            _sessionFactory = new NHibernateSessionManager();
        }

        #endregion

   
        [Test]
        public void Can_Load_Configuration_With_Mappings()
        {
            var mgr = new NHibernateSessionManager();
            ISession session = mgr.GetSession();

            Assert.IsNotNull(session);
        }

        [Test]
        public void Can_Persist_Cat()
        {
            var originalCat = new Cat
                                  {
                                      Name = "Jade Pants",
                                      Sex = 'M',
                                      Weight = 2
                                  };
            var id = _provider.Save(originalCat);

            var savedCat = _provider.GetById(id);

            Assert.AreEqual(id, savedCat.Id);
        }

        [Test, Explicit]
        public void Export_Schema()
        {
            NHibernateSessionManager.ExportSchema();
        }
    }
}