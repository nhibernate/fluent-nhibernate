using System.Diagnostics;
using NUnit.Framework;
using ShadeTree.DomainModel;

namespace ShadeTree.Testing.DomainModel
{
    [TestFixture]
    public class InMemoryRepositoryTester
    {
        [Test]
        public void Store_and_find_two_different_types_of_objects()
        {
            InMemoryRepository repository = new InMemoryRepository();
            repository.Save(new Case {Id = 2});
            repository.Save(new Case {Id = 3});
            repository.Save(new Contact {Id = 2});
            repository.Save(new Contact {Id = 3});

            repository.Find<Case>(2).Id.ShouldEqual(2);
            repository.Find<Contact>(3).ShouldBeOfType(typeof(Contact));
            repository.Find<Case>(3).ShouldBeOfType(typeof(Case));
        }

        [Test]
        public void Query_by_expression()
        {
            InMemoryRepository repository = new InMemoryRepository();
            repository.Save(new Case{Name = "Jeremy", Number = 10});
            repository.Save(new Case{Name = "Darth Vader", Number = 5});
            repository.Save(new Case{Name = "Darth Maul", Number = 6});
            repository.Save(new Case{Name = "Luke", Number = 12});
            repository.Save(new Case{Name = "Han Solo", Number = 6});
            repository.Save(new Case{Name = "Eric", Number = 5});
            repository.Save(new Case{Name = "Corwin", Number = 4});

            repository.Query<Case>(c => c.Name.StartsWith("Darth")).Length.ShouldEqual(2);
            repository.Query<Case>(c => c.Number == 6).Length.ShouldEqual(2);
            Case[] cases = repository.Query<Case>(c => c.Number > 5);
            cases.Length.ShouldEqual(4);
        }

        [Test]
        public void Find_by_property()
        {
            InMemoryRepository repository = new InMemoryRepository();
            repository.Save(new Case { Name = "Jeremy", Number = 10 });
            repository.Save(new Case { Name = "Darth Vader", Number = 5 });
            repository.Save(new Case { Name = "Darth Maul", Number = 6 });
            repository.Save(new Case { Name = "Luke", Number = 12 });
            repository.Save(new Case { Name = "Han Solo", Number = 6 });
            repository.Save(new Case { Name = "Eric", Number = 5 });
            repository.Save(new Case { Name = "Corwin", Number = 4 });

            repository.FindBy<Case, string>(c => c.Name, "Jeremy").Name.ShouldEqual("Jeremy");
        }

        [Test]
        public void Find_by_query()
        {
            InMemoryRepository repository = new InMemoryRepository();
            repository.Save(new Case { Name = "Jeremy", Number = 10 });
            repository.Save(new Case { Name = "Darth Vader", Number = 5 });
            repository.Save(new Case { Name = "Darth Maul", Number = 6 });
            repository.Save(new Case { Name = "Luke", Number = 12 });
            repository.Save(new Case { Name = "Han Solo", Number = 6 });
            repository.Save(new Case { Name = "Eric", Number = 5 });
            repository.Save(new Case { Name = "Corwin", Number = 4 });

            repository.FindBy<Case>(c => c.Name == "Corwin").Name.ShouldEqual("Corwin");
            repository.FindBy<Case>(c => c.Number == 10).Name.ShouldEqual("Jeremy");
            repository.FindBy<Case>(c => c.Number == 6 && c.Name == "Han Solo").Name.ShouldEqual("Han Solo");
        }

    }

    public class Case : Entity
    {
        public long Number { get; set; }
        public string Name { get; set; }
    }

    public class Contact : Entity
    {
    }
}