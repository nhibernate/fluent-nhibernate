using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FluentNHibernate.Framework;
using FluentNHibernate.Framework.Fixtures;
using FluentNHibernate.Framework.Query;
using NHibernate;
using NUnit.Framework;
using FluentNHibernate.Mapping;
using NHibernate.Linq;
using StructureMap;

namespace FluentNHibernate.Testing.DomainModel
{
    [TestFixture, Explicit]
    public class ConnectedTester
    {
        private SessionSource _source;

        [SetUp]
        public void SetUp()
        {
            IDictionary<string, string> props = new Dictionary<string, string>();
            props.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            props.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
            props.Add("dialect", "NHibernate.Dialect.MsSql2000Dialect");
            props.Add("hibernate.dialect", "NHibernate.Dialect.MsSql2000Dialect");
            props.Add("use_outer_join", "true");
            props.Add("connection.connection_string", "Data Source=.;Initial Catalog=ShadeTree;Integrated Security=True;Pooling=False");
            //props.Add("show_sql", showSql);
            props.Add("show_sql", true.ToString());

            _source = new SessionSource(props, new TestModel());

            _source.BuildSchema();

            ObjectFactory.Inject<ISessionSource>(_source);
            DomainObjectFinder.ClearAllFinders();
        }

        [TearDown]
        public void TearDown()
        {
            ObjectFactory.ResetDefaults();
        }


        [Test]
        public void Spin_up_the_Linq_stuff()
        {
            ISession session = _source.CreateSession();

            session.SaveOrUpdate(new Record{Name = "Jeremy", Age = 34});
            session.SaveOrUpdate(new Record{Name = "Jessica", Age = 29});
            session.SaveOrUpdate(new Record{Name = "Natalie", Age = 25});
            session.SaveOrUpdate(new Record{Name = "Hank", Age = 29});
            session.SaveOrUpdate(new Record{Name = "Darrell", Age = 34});
            session.SaveOrUpdate(new Record{Name = "Bill", Age = 34});
            session.SaveOrUpdate(new Record{Name = "Tim", Age = 35});
            session.SaveOrUpdate(new Record{Name = "Greg", Age = 36});

            //ISession session2 = _source.CreateSession();
            //var query = from record in session2.Linq<Record>() where record.Age < 30 select record;
            //foreach (Record record in query.ToList())
            //{
            //    Debug.WriteLine(record.Name);
            //}

            //return;

            Repository repository = new Repository(_source.CreateSession());
            Record[] records = repository.Query<Record>(record => record.Age == 29);
            //records.Length.ShouldEqual(2);

            foreach (var record in records)
            {
                Debug.WriteLine(record.Name);
            }
        }

        [Test]
        public void QueryBy_test()
        {
            ISession session = _source.CreateSession();

            session.SaveOrUpdate(new Record { Name = "Jeremy", Age = 34 });
            session.SaveOrUpdate(new Record { Name = "Jessica", Age = 29 });
            session.SaveOrUpdate(new Record { Name = "Natalie", Age = 25 });
            session.SaveOrUpdate(new Record { Name = "Hank", Age = 29 });
            session.SaveOrUpdate(new Record { Name = "Darrell", Age = 34 });
            session.SaveOrUpdate(new Record { Name = "Bill", Age = 34 });
            session.SaveOrUpdate(new Record { Name = "Tim", Age = 35 });
            session.SaveOrUpdate(new Record { Name = "Greg", Age = 36 });

            Repository repository = new Repository(_source.CreateSession());
            Record record = repository.FindBy<Record, string>(r => r.Name, "Hank");
            record.Name.ShouldEqual("Hank");
        }

        [Test]
        public void Query_by_filters()
        {
            ISession session = _source.CreateSession();

            session.SaveOrUpdate(new Record { Name = "Jeremy", Age = 34 });
            session.SaveOrUpdate(new Record { Name = "Jeremy", Age = 35 });
            session.SaveOrUpdate(new Record { Name = "Jessica", Age = 29 });
            session.SaveOrUpdate(new Record { Name = "Natalie", Age = 25 });
            session.SaveOrUpdate(new Record { Name = "Hank", Age = 29 });
            session.SaveOrUpdate(new Record { Name = "Darrell", Age = 34 });
            session.SaveOrUpdate(new Record { Name = "Bill", Age = 34 });
            session.SaveOrUpdate(new Record { Name = "Tim", Age = 35 });
            session.SaveOrUpdate(new Record { Name = "Greg", Age = 36 });

            Repository repository = new Repository(_source.CreateSession());
            Record record = repository.FindBy<Record>(r => r.Age == 34 && r.Name == "Jeremy");
            record.Name.ShouldEqual("Jeremy");
            record.Age.ShouldEqual(34);
        }

        [Test]
        public void Query_with_multiple_expressions()
        {
            ISession session = _source.CreateSession();

            session.SaveOrUpdate(new Record { Name = "Jeremy", Age = 34 });
            session.SaveOrUpdate(new Record { Name = "Jeremy", Age = 35 });

            Repository repository = new Repository(_source.CreateSession());
            Record[] records = repository.Query<Record>(r => r.Age >= 35 && r.Name == "Jeremy");

            records.ShouldHaveCount(1);
            records[0].Name.ShouldEqual("Jeremy");
            records[0].Age.ShouldEqual(35);
        }

        [Test]
        public void Try_out_EntityQuery()
        {
            var queryDef = new EntityQueryDefinitionBuilder<Record>()
                .AllowFilterOn(r => r.Age)
                .AllowFilterOn(r => r.Name)
                .QueryDefinition;

            ISession session = _source.CreateSession();

            session.SaveOrUpdate(new Record { Name = "Jeremy", Age = 34 });
            session.SaveOrUpdate(new Record { Name = "Jeremy", Age = 35 });

            Repository repository = new Repository(_source.CreateSession());

            var query = new EntityQueryBuilder<Record>(queryDef);
            query.AddFilter(new BinaryFilterType { FilterExpressionType = ExpressionType.Equal }, queryDef.GetFilterPropertyForKey("Age"), "35");
            query.AddFilter(new StringFilterType { StringMethod = s => s.EndsWith("") }, queryDef.GetFilterPropertyForKey("Name"), "emy");

            Record[] records = repository.Query(query.FilterExpression);

            records.ShouldHaveCount(1);
            records[0].Name.ShouldEqual("Jeremy");
            records[0].Age.ShouldEqual(35);
        }

        [Test]
        public void Try_out_DomainObjectFinder()
        {
            ISession session = _source.CreateSession();

            session.SaveOrUpdate(new Record { Name = "Jeremy", Age = 34 });
            session.SaveOrUpdate(new Record { Name = "Jeremy", Age = 35 });
            session.SaveOrUpdate(new Record { Name = "Jessica", Age = 29 });
            session.SaveOrUpdate(new Record { Name = "Natalie", Age = 25 });
            session.SaveOrUpdate(new Record { Name = "Hank", Age = 29 });
            session.SaveOrUpdate(new Record { Name = "Darrell", Age = 34 });
            session.SaveOrUpdate(new Record { Name = "Bill", Age = 34 });
            session.SaveOrUpdate(new Record { Name = "Chad", Age = 35 });
            session.SaveOrUpdate(new Record { Name = "Earl", Age = 36 });

            Repository repository = new Repository(_source.CreateSession());
            ObjectFactory.Inject<IRepository>(repository);

            
            
            
            DomainObjectFinder.Type<Record>().IsFoundByProperty(r => r.Name);
            
            
            
            
            Record record = DomainObjectFinder.Find<Record>("Chad");
            record.Name.ShouldEqual("Chad");
        }


        [Test]
        public void Try_out_DomainObjectFinder2()
        {
            ISession session = _source.CreateSession();

            session.SaveOrUpdate(new Record { Name = "Jeremy", Age = 34 });
            session.SaveOrUpdate(new Record { Name = "Jeremy", Age = 35 });
            session.SaveOrUpdate(new Record { Name = "Jessica", Age = 29 });
            session.SaveOrUpdate(new Record { Name = "Natalie", Age = 25 });
            session.SaveOrUpdate(new Record { Name = "Hank", Age = 29 });
            session.SaveOrUpdate(new Record { Name = "Darrell", Age = 34 });
            session.SaveOrUpdate(new Record { Name = "Bill", Age = 34 });
            session.SaveOrUpdate(new Record { Name = "Chad", Age = 35 });
            session.SaveOrUpdate(new Record { Name = "Earl", Age = 36 });

            Repository repository = new Repository(_source.CreateSession());
            ObjectFactory.Inject<IRepository>(repository);




            DomainObjectFinder.Type<Record>().IsFoundBy(name =>
            {
                return new Record{Name = name};
            });


            Record record = DomainObjectFinder.Find<Record>("Chad");
            record.Name.ShouldEqual("Chad");
        }

        [Test]
        public void Save_and_find()
        {
            Repository repository1 = new Repository(_source.CreateSession());
            var record1 = new Record { Name = "Jeremy", Age = 34 };
            repository1.Save(record1);

            Repository repository2 = new Repository(_source.CreateSession());
            Record record2 = repository2.Find<Record>(record1.Id);

            record1.ShouldNotBeTheSameAs(record2);
            record2.Name.ShouldEqual("Jeremy");
        }

        [Test]
        public void MappingTest1()
        {
            new PersistenceSpecification<Record>()
                .CheckProperty(r => r.Age, 22)
                .CheckProperty(r => r.Name, "somebody")
                .CheckProperty(r => r.Location, "somebody")
                .VerifyTheMappings();
                
        }
    }

    public class TestModel : PersistenceModel
    {
        public TestModel()
        {
            addMappingsFromThisAssembly();
        }
    }

    public class RecordMap : ClassMap<Record>
    {
        public RecordMap()
        {
            UseIdentityForKey(x => x.Id, "id");
            Map(x => x.Name);
            Map(x => x.Age);
        }
    }

    public class Record : Entity
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Location { get; set; }
    }
}
