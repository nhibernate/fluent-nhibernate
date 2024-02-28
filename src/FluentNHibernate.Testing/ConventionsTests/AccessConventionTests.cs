using System;
using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Testing.DomainModel.Access;
using FluentNHibernate.Testing.DomainModel.Access.Mappings;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests;

[TestFixture]
public class AccessConventionTests
{
    private string expectedAccess = "backfield";

    private ClassMapping compositeId;
    private ClassMapping manyToMany;
    private ClassMapping manyToOne;
    private ClassMapping oneToOne;
    private ClassMapping parent;


    [SetUp]
    public void SetUp()
    {
        PersistenceModel model = new PersistenceModel();
        model.Conventions.Add(new BackfieldAccessConvention());

        model.Add(new CompositeIdModelMapping());
        model.Add(new ManyToManyModelMapping());
        model.Add(new ManyToOneModelMapping());
        model.Add(new OneToOneModelMapping());
        model.Add(new ParentModelMapping());

        var classMappings = model.BuildMappings().SelectMany(x => x.Classes).ToDictionary(x => x.Type);
        compositeId = classMappings[typeof(CompositeIdModel)];
        manyToMany = classMappings[typeof(ManyToManyModel)];
        manyToOne = classMappings[typeof(ManyToOneModel)];
        oneToOne = classMappings[typeof(OneToOneModel)];
        parent = classMappings[typeof(ParentModel)];
    }

    [Test]
    public void IdIsSet()
    {
        Assert.That(((IdMapping)parent.Id).Access, Is.EqualTo(expectedAccess));
    }

    [Test]
    public void CompositeIdIsSet()
    {
        CompositeIdMapping id;

        id = ((CompositeIdMapping)compositeId.Id);
        Assert.That(id.Access, Is.EqualTo(expectedAccess));
        Assert.That(id.Keys.First(x => x.Name.Equals("IdA")).Access, Is.EqualTo(expectedAccess));
        Assert.That(id.Keys.First(x => x.Name.Equals("IdB")).Access, Is.EqualTo(expectedAccess));

        id = ((CompositeIdMapping)oneToOne.Id);
        Assert.That(id.Access, Is.EqualTo(expectedAccess));
        Assert.That(id.Keys.First(x => x.Name.Equals("Parent")).Access, Is.EqualTo(expectedAccess));
    }

    [Test]
    public void VersionIsSet()
    {
        Assert.That(parent.Version.Access, Is.EqualTo(expectedAccess));
    }

    [Test]
    public void PropertyIsSet()
    {
        Assert.That(parent.Properties.First(x => x.Name.Equals("Property")).Access, Is.EqualTo(expectedAccess));
    }

    [Test]
    public void JoinedPropertyIsSet()
    {
        Assert.That(parent.Joins.SelectMany(x => x.Properties).First(x => x.Name.Equals("JoinedProperty")).Access, Is.EqualTo(expectedAccess));
    }

    [Test]
    public void ComponentIsSet()
    {
        Assert.That(parent.Components.First(x => x.Name.Equals("Component")).Access, Is.EqualTo(expectedAccess));
    }

    [Test]
    public void DynamicComponentIsSet()
    {
        Assert.That(parent.Components.First(x => x.Name.Equals("Dynamic")).Access, Is.EqualTo(expectedAccess));
    }

    [Test]
    public void OneToOneIsSet()
    {
        Assert.That(parent.OneToOnes.First(x => x.Name.Equals("One")).Access, Is.EqualTo(expectedAccess));
    }

    [Test]
    public void OneToManyIsSet()
    {
        Assert.That(parent.Collections.First(x => x.Name.Equals("MapOne")).Access, Is.EqualTo(expectedAccess));
        Assert.That(parent.Collections.First(x => x.Name.Equals("SetOne")).Access, Is.EqualTo(expectedAccess));
        Assert.That(parent.Collections.First(x => x.Name.Equals("ListOne")).Access, Is.EqualTo(expectedAccess));
        Assert.That(parent.Collections.First(x => x.Name.Equals("BagOne")).Access, Is.EqualTo(expectedAccess));
    }

    [Test]
    public void ManyToManyIsSet()
    {
        Assert.That(parent.Collections.First(x => x.Name.Equals("MapMany")).Access, Is.EqualTo(expectedAccess));
        Assert.That(parent.Collections.First(x => x.Name.Equals("SetMany")).Access, Is.EqualTo(expectedAccess));
        Assert.That(parent.Collections.First(x => x.Name.Equals("ListMany")).Access, Is.EqualTo(expectedAccess));
        Assert.That(parent.Collections.First(x => x.Name.Equals("BagMany")).Access, Is.EqualTo(expectedAccess));
    }

    [Test]
    public void ManyToOneIsSet()
    {
        Assert.That(manyToOne.References.First(x => x.Name.Equals("Parent")).Access, Is.EqualTo(expectedAccess));
    }

    [Test]
    public void AnyIsSet()
    {
        Assert.That(parent.Anys.First(x => x.Name.Equals("Any")).Access, Is.EqualTo(expectedAccess));
    }

    private class BackfieldAccessConvention : AccessConvention
    {
        protected override void Apply(Type owner, string name, FluentNHibernate.Conventions.Instances.IAccessInstance access)
        {
            access.BackField();
        }
    }
}
